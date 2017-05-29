using System.Collections;
using System.Collections.Generic;
using HauntedCity.GameMechanics.Main;
using UnityEngine;

public class AccelerationCleaner : MonoBehaviour {

    public int maxDepth = 250;

    public class MovementStepData
    {
        public Vector3 linearAcc;
        public Vector3 linearAccAverage;
        public Vector3 gyroRotationSpeed;
        public float linearAccMagnitude;
        public float gyroRotationSpeedMagnitude;
        public Vector3 linearAccClearedFromConstant;
        public Vector3 gyroRotationSpeedClearedFromConstant;
        public float linearAccMagnitudeClearedFromConstant;
        public float gyroRotationSpeedMagnitudeClearedFromConstant;
        public float linearAccEnergy;
        public float gyroRotationEnergy;
        public float linearAccVariance;
        public float gyroRotationVariance;
    }

    private Queue<MovementStepData> dataQueue = new Queue<MovementStepData>();
    MovementStepData lastMSD;

    public void Init(GyroControl gc)
    {
        for(int i = 0; i < maxDepth; ++i)
        {
            insertData(gc);
        }
    }

    public MovementStepData getRawData()
    {
        return lastMSD;
    }

    public Vector3 getFilteredAcceleration(out MovementStepData md)
    {
        MovementStepData msd = getRawData();
        md = msd;
        int N = dataQueue.Count;
        if (N == 0) N++;
        Vector3 outputVec = new Vector3(msd.linearAccAverage.x, msd.linearAccAverage.y, msd.linearAccAverage.z);

        float diffOfRotAndMovEnergies = 0.4f;
        if ((msd.gyroRotationEnergy - msd.linearAccEnergy) > diffOfRotAndMovEnergies)
            return Vector3.zero;

        float z = outputVec.z;
        float deltaToDistinguishFrontAndBack = 0.026f;

        if(z > 0) //if moving back
        {
            if (z < deltaToDistinguishFrontAndBack) //not strongly - forbid back movement
            {
                outputVec.z = 0;
            }
            else //strongly - enforce
            {
                outputVec.z *= 1.5f;
            }
        }

        return outputVec;
    }

    public void insertData(GyroControl gc)
    {
        Gyroscope gyro = gc.gyro;
        Vector3 linearAcc = gyro.userAcceleration;
        Vector3 gyroRotationSpeed = gyro.rotationRateUnbiased;
        //Vector3 linearAcc = new Vector3(0.1f, 0.1f, 0.2f);
        //Vector3 gyroRotationSpeed = new Vector3(0.1f, 0.1f, 0.2f);
        float linearAccMagnitude = linearAcc.magnitude;
        float gyroRotationSpeedMagnitude = gyroRotationSpeed.magnitude;

        Vector3 linearAccClearedFromConstant = new Vector3(0, 0, 0);
        Vector3 gyroRotationSpeedClearedFromConstant = new Vector3(0, 0, 0);
        Vector3 linearAccAverage = new Vector3(0, 0, 0);
        float linearAccMagnitudeClearedFromConstant = 0;
        float gyroRotationSpeedMagnitudeClearedFromConstant = 0;
        float linearAccEnergy = 0;
        float gyroRotationEnergy = 0;

        int N = dataQueue.Count;
        if (N == 0) N++;

        foreach (MovementStepData md in dataQueue)
        {
            linearAccClearedFromConstant += md.linearAcc;
            linearAccAverage += md.linearAcc;
            gyroRotationSpeedClearedFromConstant += md.gyroRotationSpeed;
            linearAccMagnitudeClearedFromConstant += md.linearAccMagnitude;
            gyroRotationSpeedMagnitudeClearedFromConstant += md.gyroRotationSpeedMagnitude;
            linearAccEnergy +=  md.linearAccMagnitudeClearedFromConstant;
            gyroRotationEnergy += md.gyroRotationSpeedMagnitudeClearedFromConstant;
        }
        linearAccClearedFromConstant = linearAcc - 1 / N * linearAccClearedFromConstant;
        linearAccAverage /= N;
        gyroRotationSpeedClearedFromConstant = gyroRotationSpeed - 1 / N * gyroRotationSpeedClearedFromConstant;
        linearAccMagnitudeClearedFromConstant = linearAccMagnitude - 1 / N * linearAccMagnitudeClearedFromConstant;
        gyroRotationSpeedMagnitudeClearedFromConstant = gyroRotationSpeedMagnitude - 1 / N * gyroRotationSpeedMagnitudeClearedFromConstant;
        linearAccEnergy /= N;
        gyroRotationEnergy /= N;


        float linearAccVariance = 0;
        float gyroRotationVariance = 0;

        foreach (MovementStepData md in dataQueue)
        {
            float curLinVarMember = md.linearAccMagnitudeClearedFromConstant + linearAccEnergy;
            linearAccVariance += curLinVarMember * curLinVarMember;
            float curRotVarMember = md.gyroRotationSpeedMagnitudeClearedFromConstant + gyroRotationEnergy;
            gyroRotationVariance += curRotVarMember * curRotVarMember;
        }
        linearAccVariance /= N;
        gyroRotationVariance /= N;

        MovementStepData mdNew = new MovementStepData();
        mdNew.gyroRotationEnergy = gyroRotationEnergy;
        mdNew.gyroRotationSpeed = gyroRotationSpeed;
        mdNew.gyroRotationSpeedClearedFromConstant = gyroRotationSpeedClearedFromConstant;
        mdNew.gyroRotationSpeedMagnitude = gyroRotationSpeedMagnitude;
        mdNew.gyroRotationSpeedMagnitudeClearedFromConstant = gyroRotationSpeedMagnitudeClearedFromConstant;
        mdNew.gyroRotationVariance = gyroRotationVariance;
        mdNew.linearAcc = linearAcc;
        mdNew.linearAccClearedFromConstant = linearAccClearedFromConstant;
        mdNew.linearAccEnergy = linearAccEnergy;
        mdNew.linearAccMagnitude = linearAccMagnitude;
        mdNew.linearAccMagnitudeClearedFromConstant = linearAccMagnitudeClearedFromConstant;
        mdNew.linearAccVariance = linearAccVariance;
        mdNew.linearAccAverage = linearAccAverage;

        if(N >= maxDepth)
        {
            dataQueue.Dequeue();
        }
        dataQueue.Enqueue(mdNew);
        lastMSD = mdNew;
}

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
