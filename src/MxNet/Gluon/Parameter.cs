﻿using MxNet.NN.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MxNet.Gluon
{
    public class Parameter
    {
        public float LRMult { get; set; }

        public float WDMult { get; set; }

        public Parameter(string name, string grad_req= "write", Shape shape= null, string dtype= "float32",
                 float lr_mult= 1.0f, float wd_mult= 1.0f, BaseInitializer init= null, bool allow_deferred_init= false,
                 bool differentiable= true, string stype= "default", string grad_stype= "default")
        {
            throw new NotImplementedException();
        }
    }
}