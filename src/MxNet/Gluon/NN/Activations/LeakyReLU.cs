﻿/*****************************************************************************
   Copyright 2018 The MxNet.Sharp Authors. All Rights Reserved.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
******************************************************************************/
using System;

namespace MxNet.Gluon.NN
{
    public class LeakyReLU : HybridBlock
    {
        public LeakyReLU(float alpha) : base()
        {
            if (alpha < 0)
                throw new ArgumentException("Slope coefficient for LeakyReLU must be no less than 0");

            Alpha = alpha;
        }

        public float Alpha { get; set; }

        public override NDArrayOrSymbolList HybridForward(NDArrayOrSymbolList args)
        {
            var x = args[0];
            if (x.IsNDArray)
                return nd.LeakyReLU(x.NdX, slope: Alpha);

            return sym.LeakyReLU(x.SymX, slope: Alpha);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Alpha})";
        }
    }
}