﻿using System;
using System.Collections.Generic;
using System.Text;
using MxNet;
using MxNet.Gluon;
using MxNet.Gluon.NN;
using MxNet.Initializers;
using MxNet.Numpy;

namespace BasicExamples
{
    public class CrashCourse_NDArray
    {
        private static ndarray F(ndarray a)
        {
            ndarray c = null;
            var b = a * 2;
            while (np.linalg.norm(b).asscalar() < 1000)
                b = b * 2;

            if (b.sum().asscalar() >= 0)
                c = b[0];
            else
                c = b[1];

            return c;
        }

        public static void GetStarted()
        {
            var net = new Sequential();

            // Similar to Dense, it is not necessary to specify the input channels
            // by the argument `in_channels`, which will be  automatically inferred
            // in the first forward pass. Also, we apply a relu activation on the
            // output. In addition, we can use a tuple to specify a  non-square
            // kernel size, such as `kernel_size=(2,4)`
            net.Add(new Conv2D(channels: 6, kernel_size: (5, 5), activation: ActivationType.Relu),
                // One can also use a tuple to specify non-symmetric pool and stride sizes
                new MaxPool2D(pool_size: (2, 2), strides: (2, 2)),
                new Conv2D(channels: 16, kernel_size: (3, 3), activation: ActivationType.Relu),
                new MaxPool2D(pool_size: (2, 2), strides: (2, 2)),
                // The dense layer will automatically reshape the 4-D output of last
                // max pooling layer into the 2-D shape: (x.shape[0], x.size/x.shape[0])
                new Dense(120, activation: ActivationType.Relu),
                new Dense(84, activation: ActivationType.Relu),
                new Dense(10)
                );

            net.Initialize();
            // Input shape is (batch_size, color_channels, height, width)
            var x = np.random.uniform(size: new Shape(4, 1, 28, 28));
            ndarray y = net.Call(x);
            Console.WriteLine(y.shape);

            Console.WriteLine(net[0].Params["weight"].Data().shape);
            Console.WriteLine(net[5].Params["bias"].Data().shape);

            //var net = new MixMLP();
            //net.Initialize();
            //var x = nd.Random.Uniform(shape: new Shape(2, 2));
            //var y = net.Call(x);

            //net.blk[1].Params["weight"].Data();
        }
    }

    public class MixMLP : Block
    {
        public Sequential blk;
        public Block dense;

        public MixMLP() : base()
        {
            blk = new Sequential();
            blk.Add(new Dense(3, activation: ActivationType.Relu),
                    new Dense(4, activation: ActivationType.Relu));

            dense = new Dense(5);
        }

        public override NDArrayOrSymbolList Forward(NDArrayOrSymbolList args)
        {
            var y = npx.relu(this.blk.Call(args)[0]);
            Console.WriteLine(y);
            return this.dense.Call(y);
        }
    }
}
