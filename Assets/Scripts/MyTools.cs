using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools
{
    public class Tools : MonoBehaviour
    {
        public struct RandomSelection
        {
            private float minValue, maxValue;
            public float probability;

            public RandomSelection(int minValue, int maxValue, float probability)
            {
                this.minValue = (float)minValue;
                this.maxValue = (float)maxValue;
                this.probability = probability;
                this.probability = probability;
            }

            public RandomSelection(float minValue, float maxValue, float probability)
            {
                this.minValue = minValue;
                this.maxValue = maxValue;
                this.probability = probability;
            }

            public int GetValueInt() { return Random.Range((int)minValue, (int)maxValue + 1); }
            public float GetValueFloat() { return Random.Range(minValue, maxValue + 1f); }

        }

        public static int GetRandomValueInt(params RandomSelection[] selections)
        {
            float rand = Random.value;
            float currentProb = 0;
            foreach (var selection in selections)
            {
                currentProb += selection.probability;
                if (rand <= currentProb) return selection.GetValueInt();
            }

            //will happen if the input's probabilities sums to less than 1
            //throw error here if that's appropriate
            return -1;
        }


        public static float GetRandomValueFloat(params RandomSelection[] selections)
        {
            float rand = Random.value;
            float currentProb = 0;
            foreach (var selection in selections)
            {
                currentProb += selection.probability;
                if (rand <= currentProb) return selection.GetValueFloat();
            }

            return -1;
        }

        public static void SetColor(GameObject tarObj, Color color)
        {
            if (tarObj.GetComponent<Renderer>() == null)
            {
                var _rend = tarObj.GetComponentInChildren<Renderer>();
                if (_rend.GetComponent<MaterialColorChanger>() == null) _rend.gameObject.AddComponent<MaterialColorChanger>();
                _rend.GetComponent<MaterialColorChanger>().SetColor(_rend, color);
                return;
            }

            Renderer rend = tarObj.GetComponent<Renderer>();
            rend.GetComponent<MaterialColorChanger>().
                SetColor(rend, color);
        }
    }

}
