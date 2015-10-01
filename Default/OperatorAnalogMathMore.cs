﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using LabNation.Interfaces;

namespace LabNation.Decoders
{
    [Export(typeof(IProcessor))]
    public class OperatorAnalogMathMore : IOperatorAnalog
    {
        public DecoderDescription Description
        {
            get
            {
                return new DecoderDescription()
                {
                    Name = "Math - More",
                    ShortName = "MAT",
                    Author = "LabNation",
                    VersionMajor = 0,
                    VersionMinor = 1,
                    Description = "Extended math operator",
                    InputWaveformTypes = new Dictionary<string, Type>() 
                    {
                        { "In0", typeof(float)},
                        { "In1", typeof(float)}
                    },
                    Parameters = new DecoderParameter[]
                    {
                        new DecoderParamaterInts("Offset", new int[] { -2, -1, 0 ,1 ,2}, "", 0, "Offset"),                        
                        new DecoderParamaterStrings("Op1", new string[] { "+", "-", "*", "/", "Mod" }, "+", "Operator"),
                        new DecoderParamaterStrings("Dum1", new string[] { "(" }, "(", "Dummy1"),
                        new DecoderParamaterInts("Sc0", new int[] { -2, -1, 0 ,1 ,2}, "", 1, "Input0 scaler"),
                        new DecoderParamaterStrings("Op2", new string[] { "+", "-", "*", "/", "Mod" }, "+", "Operator"),
                        new DecoderParamaterInts("Sc1", new int[] { -2, -1, 0 ,1 ,2}, "", 1, "Input1 scaler"),
                        new DecoderParamaterStrings("Dum2", new string[] { ")" }, ")", "Dummy2"),
                    },
                    ContextMenuOrder = new List<string>(new string[] {"Offset", "Op1", "Dum1", "Sc0", "In0", "Op2", "Sc1", "In1", "Dum2"})
                };
            }
        }

        public float[] Process(Dictionary<string, Array> inputWaveforms, Dictionary<string, object> parameters, double samplePeriod)
        {
            //name input waveforms for easier usage
            float[] i0 = (float[])inputWaveforms["In0"];
            float[] i1 = (float[])inputWaveforms["In1"];

            //fetch operators
            string op1 = (string)parameters["Op1"];
            string op2 = (string)parameters["Op2"];
            int offset = (int)parameters["Offset"];
            int sc0 = (int)parameters["Sc0"];
            int sc1 = (int)parameters["Sc1"];

            //allocate output buffer
            float[] output = new float[i0.Length];
            float[] intern = new float[i0.Length];
            
            if (op2 == "+")
                for (int i = 0; i < i0.Length; i++)
                    intern[i] = (float)sc0*i0[i] + (float)sc1*i1[i];
            else if (op2 == "-")
                for (int i = 0; i < i0.Length; i++)
                    intern[i] = (float)sc0 * i0[i] - (float)sc1 * i1[i];
            else if (op2 == "*")
                for (int i = 0; i < i0.Length; i++)
                    intern[i] = (float)sc0 * i0[i] * (float)sc1 * i1[i];
            else if (op2 == "/")
                for (int i = 0; i < i0.Length; i++)
                    intern[i] = (float)sc0 * i0[i] / ((float)sc1 * i1[i]);
            else if (op2 == "Mod")
                for (int i = 0; i < i0.Length; i++)
                    intern[i] = (float)sc0 * i0[i] % ((float)sc1 * i1[i]);

            if (op1 == "+")
                for (int i = 0; i < i0.Length; i++)
                    output[i] = (float)offset + intern[i];
            else if (op1 == "-")
                for (int i = 0; i < i0.Length; i++)
                    output[i] = (float)offset - intern[i];
            else if (op1 == "*")
                for (int i = 0; i < i0.Length; i++)
                    output[i] = (float)offset * intern[i];
            else if (op1 == "/")
                for (int i = 0; i < i0.Length; i++)
                    output[i] = (float)offset / intern[i];
            else if (op1 == "Mod")
                for (int i = 0; i < i0.Length; i++)
                    output[i] = (float)offset % intern[i];

            return output;
        }
    }
}
