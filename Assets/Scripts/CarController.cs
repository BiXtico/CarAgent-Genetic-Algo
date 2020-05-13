using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(NNetwork))]
public class CarController : MonoBehaviour
{
    [Range(-1f,1f)]
    public float acceleration,turnningval;
    private NNetwork EnvironmentControler;
    public Vector3 SP, SR;
    public float time = 0f;

    [Header("PerformanceFun")]  
    public float overallPerformance;

    [Header("EnvironmentControler")] 
    public int LAYER = 1;
    public int NEURON = 10;
    
    
    private float distanceMul = 1.4f;
    private float avgSpeedMul = 0.2f;
    private float sensorMul = 0.1f;
    private float smootherMul = 0.02f;
    private Vector3 lastPosition;
    private float totalDistance;
    private float avgSpeed;
    private float normalizeval = 60;

    private float a,b,c;

    private void Awake() {
        SP = transform.position;
        SR = transform.eulerAngles;
        EnvironmentControler = GetComponent<NNetwork>();
        
        
        EnvironmentControler.Initialise(LAYER,NEURON);
    }

    public void ResetWithNetwork(NNetwork net)
    {
        EnvironmentControler = net;
        ResetCar();
    }

    public void ResetCar() {
        EnvironmentControler.Initialise(LAYER,NEURON);
        transform.position = SP;
        transform.eulerAngles = SR;
        time = 0f;
        totalDistance = 0f;
        avgSpeed = 0f;
        lastPosition = SP;
        overallPerformance = 0f;
    }

    private void OnCollisionEnter (Collision collision) {
        if(collision.gameObject.CompareTag("danger"))
            Death();
    }

    private void FixedUpdate() {

        InputSensors();
        lastPosition = transform.position;

        (acceleration, turnningval) = EnvironmentControler.RunNetwork(a, b, c);

        CarCon(acceleration,turnningval);

        time += Time.deltaTime;

        CalculateFitness();
    }
    private Vector3 temp; 
    public void CarCon (float positionvar, float anglevar) {
        temp = Vector3.Lerp(Vector3.zero,new Vector3(0,0,positionvar*11.4f),smootherMul);
        temp = transform.TransformDirection(temp); 
        transform.position += temp;
        transform.eulerAngles += new Vector3(0, (anglevar*90)*smootherMul,0); 
    }

    private void CalculateFitness() {

        totalDistance += Vector3.Distance(transform.position,lastPosition);
        avgSpeed = totalDistance/time;
        overallPerformance = (avgSpeed*avgSpeedMul)+(totalDistance*distanceMul)+(((a+b+c)/3)*sensorMul);

        if (time > 20 && overallPerformance < 35) {
            Death();
        }

        if (overallPerformance >= 1000) {
            Debug.Log("I did it");
            //Death();
        }

    }
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }
    private void Death()
    {
        FindObjectOfType<GeneticSystem>().Death(overallPerformance, EnvironmentControler);
    }

    private void InputSensors() {
        var forward = transform.forward;
        Vector3 aP = (forward+transform.right);
        Vector3 bP = (forward);
        Vector3 cP = (forward-transform.right);

        Ray r = new Ray(transform.position,aP);
        RaycastHit hit;

        if (Physics.Raycast(r,out hit))
        {
            a = hit.distance / normalizeval; 
            Debug.DrawLine(r.origin,hit.point,Color.yellow);
        }
        r.direction = cP;
        if (Physics.Raycast(r, out hit)) {
            c = hit.distance/normalizeval;
            Debug.DrawLine(r.origin,hit.point,Color.red);
        }
        r.direction = bP;

        if (Physics.Raycast(r, out hit))
        {
            b = hit.distance / normalizeval;
            Debug.DrawLine(r.origin,hit.point,Color.blue);
        }
        

    }

}
