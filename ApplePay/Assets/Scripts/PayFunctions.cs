using UnityEngine;
using System.Linq;

namespace Pay.Functions
{
    public static class Math
    {
        ///<summary>
        ///Returns angle between two vectors in degrees. 
        ///</summary>
        public static float Atan3(float y, float x) => Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f;
        ///<summary> Returns random cross vector (Vector2.up, Vector2.down etc.).</summary>
        public static Vector2 GetRandomCrossVector()
        {
            int rand = Random.Range(0, 4);
            switch(rand)
            {
                case 0:
                    return Vector2.right;
                case 1:
                    return Vector2.left;
                case 2:
                    return Vector2.down;
                default:
                    return Vector2.up;
            }
        }
        ///<summary> Returns random normalized vector. </summary>
        public static Vector2 GetRandomVector()
        {
            return new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
        }
        ///<summary>
        ///Returns random normalized vector (sum of its abs vector components value are equals to 1).
        ///</summary>
        public static Vector2 GetRandomFixedVector()
        {
            Vector2 randVec = Vector2.zero;
            randVec.x = Random.Range(-1f, 1f);
            randVec.y = (1 - Mathf.Abs(randVec.x)) * Mathf.Sign(Random.Range(-1f, 1f));
            return randVec;
        }
        ///<summary>
        ///Returns a minimal byte that specified array doesn't contain.
        ///</summary>
        public static byte GetUniqueByte(byte[] numberArray, params byte[] exclusive)
        {
            byte[] exclusivesNumber = Pay.Functions.Generic.CombineArrays(numberArray, exclusive);
            for(byte i = 0; ; i++)
            {
                if(exclusivesNumber.Length == 0)
                {
                    return i;
                }
                for(int j = 0; j < exclusivesNumber.Length; j++)
                {
                    if(exclusivesNumber[j] == i)
                        break;
                    if(exclusivesNumber.Length == j + 1)
                    {
                        return i;
                    }
                }
            }
        }
        ///<summary>Clamps length of the compenents of specified vector.</summary>
        public static Vector2 ClampVectorComponents(Vector2 vector, float min, float max) => new Vector2(Mathf.Clamp(Mathf.Abs(vector.x), min, max), Mathf.Clamp(Mathf.Abs(vector.y), min, max));
    ///<summary>
    ///Rotates vector along Z axis.
    ///</summary>
    public static Vector2 RotateVector(Vector2 vector, float angle) => Quaternion.AngleAxis(angle, Vector3.forward) * vector;
    public enum AngleType
    {
        Rad,
        Deg
    }
    public enum VectorComponent2D
    {
        X,
        Y
    }
    }
    public static class Generic
    {
        public enum FadeStatus
        {
            In,
            Const,
            Out
        }
        ///<summary>Sorts elements of specified array either from min to max value or from max to min.</summary>
        public static void BubbleSort(bool minToMax, ref float[] array) => array = BubbleSort(minToMax, array.Select(x => (double)x).ToArray()).Select(x => (float)x).ToArray();
        ///<summary>Sorts elements of specified array either from min to max value or from max to min.</summary>
        public static void BubbleSort(bool minToMax, ref int[] array) => array = BubbleSort(minToMax, array.Select(x => (double)x).ToArray()).Select(x => (int)x).ToArray();
        ///<summary>Sorts elements of specified array either from min to max value or from max to min.</summary>
        public static void BubbleSort(bool minToMax, ref byte[] array) => array = BubbleSort(minToMax, array.Select(x => (double)x).ToArray()).Select(x => (byte)x).ToArray();
        private static double[] BubbleSort(bool minToMax, double[] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                for(int j = i + 1; j < array.Length; j++)
                {
                    if(array[j] < array[i])
                    {
                        double temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
            if(!minToMax) System.Array.Reverse(array);
            return array;
        }
        ///<summary>
        ///Sorts the vectors in the array comparing only specified component of the vector.
        ///</summary>
        public static void SortVectors(ref Vector2[] array, bool minToMax, Pay.Functions.Math.VectorComponent2D sortComponent)
        {
            for(int i = 0; i < array.Length; i++)
            {
                for(int j = i + 1; j < array.Length; j++)
                {
                    if((sortComponent == Pay.Functions.Math.VectorComponent2D.X && array[j].x > array[i].x) ||
                       (sortComponent == Pay.Functions.Math.VectorComponent2D.Y && array[j].y > array[i].y)
                    ) continue;
                    Vector2 temp = array[j];
                    array[j] = array[i];
                    array[i] = temp;
                }
            }
            if(!minToMax) System.Array.Reverse(array);
        }
        ///<summary>
        ///Sorts the vectors in the array comparing only specified component of the vector.
        ///</summary>
        public static Vector2[] SortVectors(Vector2[] array, bool minToMax, Pay.Functions.Math.VectorComponent2D sortComponent)
        {
            SortVectors(ref array, minToMax, sortComponent);
            return array;
        }
        ///<summary>
        ///Combines specified array to one.
        ///</summary>
        public static byte[] CombineArrays(params byte[][] arraysToMerge)
        {
            int copyIndex = 0; 
            int arrayLength = 0;

            foreach(byte[] arr in arraysToMerge) arrayLength += arr.Length;

            byte[] resultArray = new byte[arrayLength];
            
            foreach(byte[] array in arraysToMerge)
            {
                array.CopyTo(resultArray, copyIndex);
                copyIndex += array.Length;
            }
            return resultArray;
        }
        ///<summary>
        ///Returns current mouse position.
        ///</summary>
        public static Vector2 GetMousePos(UnityEngine.Camera camera) => camera.ScreenToWorldPoint(Input.mousePosition);
        ///<summary>
        ///Converts integer from number to roman
        ///</summary>
        public static string RomanConverter(int number)
        {
            string resultChar = "";
            if(number < 0)
            {
                number *= -1;
                resultChar += "-";
            }
            for(int i = RomanNumbers.Count - 1; i >= 0; i--)
            {
                while(number - RomanNumbers.ElementAt(i).Key >= 0)
                {
                    resultChar += RomanNumbers.ElementAt(i).Value;
                    number -= RomanNumbers.ElementAt(i).Key;
                }
            }
            
            return resultChar;
        }
        ///<summary>
        ///Returns aspect ratio of the camera.
        ///</summary>
        public static Vector2 GetAspectRatio(UnityEngine.Camera camera)
        {
            Vector2 cameraResolution = new Vector2(camera.scaledPixelWidth, camera.scaledPixelHeight);
            Vector2 aspectRatio = new Vector2(cameraResolution.x/cameraResolution.y, cameraResolution.y/cameraResolution.x);
            return aspectRatio;
        }
        public static System.Collections.Generic.Dictionary<int, string> RomanNumbers = new System.Collections.Generic.Dictionary<int, string>()
        {
            [1] = "I",
            [4] = "IV",
            [5] = "V",
            [9] = "IX",
            [10] = "X",
            [40] = "XL",
            [50] = "L",
            [90] = "XC",
            [100] = "C",
            [400] = "CD",
            [500] = "D",
            [900] = "CM",
            [1000] = "M"
        };
    }
    public static class String
    {
        ///<summary>
        ///Sets two strings on sides of the wrapping word.
        ///</summary>
        public static string SetSides(string wrapping, string first, string second)
        {
            return first + wrapping + second;
        }
    }
    
}
