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
using MxNet.ND.Numpy;
using MxNet.Numpy;
using MxNet.Sym.Numpy;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MxNet
{
    public class NDArrayOrSymbol
    {
        private readonly NDArrayList ndx;

        private readonly SymbolList symx;

        public NDArrayOrSymbol(params ndarray[] x)
        {
            IsNDArray = true;
            IsSymbol = false;
            ndx = x;
        }

        internal NDArrayOrSymbol(params _Symbol[] x)
        {
            IsNDArray = false;
            IsSymbol = true;
            symx = x;
        }

        internal NDArrayOrSymbol(params Symbol[] x)
        {
            IsNDArray = false;
            IsSymbol = true;
            symx = x.Select(x => new _Symbol(x.NativePtr)).ToArray();
        }

        public bool IsSymbol { get; set; }

        public bool IsNDArray { get; set; }

        public ndarray NdX
        {
            get
            {
                if (IsNDArray)
                    return ndx;

                return null;
            }
        }

        public NDArrayList NdXList
        {
            get
            {
                if (IsNDArray)
                    return ndx;

                return null;
            }
        }

        internal _Symbol SymX
        {
            get
            {
                if (IsSymbol)
                    return symx;

                return null;
            }
        }

        internal SymbolList SymXList
        {
            get
            {
                if (IsSymbol)
                    return symx;

                return null;
            }
        }

        public NDArrayOrSymbol this[int index]
        {
            get
            {
                if (IsNDArray)
                    return ndx[index];

                return symx[index];
            }
        }

        public static NDArrayOrSymbol One
        {
            get
            {
                return 1f;
            }
        }

        public static NDArrayOrSymbol Zero
        {
            get
            {
                return 0f;
            }
        }

        public static implicit operator NDArrayOrSymbol(ndarray x)
        {
            if (x == null)
                return null;
            return new NDArrayOrSymbol(x);
        }

        public static implicit operator NDArrayOrSymbol(NDArray x)
        {
            if (x == null)
                return null;
            return new NDArrayOrSymbol(x);
        }

        public static implicit operator NDArrayOrSymbol(_Symbol x)
        {
            if (x == null)
                return null;
            return new NDArrayOrSymbol(x);
        }

        public static implicit operator NDArrayOrSymbol(Symbol x)
        {
            if (x == null)
                return null;
            return new NDArrayOrSymbol(new _Symbol(x.NativePtr));
        }

        public static implicit operator ndarray(NDArrayOrSymbol x)
        {
            if (x == null)
                return null;
            return x.NdX;
        }

        public static implicit operator _Symbol(NDArrayOrSymbol x)
        {
            if (x == null)
                return null;
            return x.SymX;
        }

        public static implicit operator NDArrayOrSymbol(float x)
        {
            ndarray array = new ndarray(new float[] { x });
            return array;
        }

        public void Deconstruct(out NDArrayOrSymbol x0, out NDArrayOrSymbol x1)
        {
            x0 = this[0];
            x1 = this[1];
        }

        public void Deconstruct(out NDArrayOrSymbol x0, out NDArrayOrSymbol x1, out NDArrayOrSymbol x2)
        {
            x0 = this[0];
            x1 = this[1];
            x2 = this[2];
        }

        public void Deconstruct(out NDArrayOrSymbol x0, out NDArrayOrSymbol x1, out NDArrayOrSymbol x2, out NDArrayOrSymbol x3)
        {
            x0 = this[0];
            x1 = this[1];
            x2 = this[2];
            x3 = this[3];
        }

        #region Operators

        public static NDArrayOrSymbol operator +(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if(lhs.IsNDArray)
                return nd_np_ops.add(lhs, rhs);

            return sym_np_ops.add(lhs, rhs);
        }

        public static NDArrayOrSymbol operator +(NDArrayOrSymbol lhs, float scalar)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.add(lhs, scalar);

            return sym_np_ops.add(lhs, scalar);
        }

        public static NDArrayOrSymbol operator +(float scalar, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.add(rhs, scalar);

            return sym_np_ops.add(rhs, scalar);
        }

        public static NDArrayOrSymbol operator -(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.subtract(lhs, rhs);

            return sym_np_ops.subtract(lhs, rhs);
        }

        public static NDArrayOrSymbol operator -(NDArrayOrSymbol lhs, float scalar)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.subtract(lhs, scalar);

            return sym_np_ops.subtract(lhs, scalar);
        }

        public static NDArrayOrSymbol operator -(float scalar, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.subtract(rhs, scalar);

            return sym_np_ops.subtract(rhs, scalar);
        }

        public static NDArrayOrSymbol operator *(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.multiply(lhs, rhs);

            return sym_np_ops.multiply(lhs, rhs);
        }

        public static NDArrayOrSymbol operator *(NDArrayOrSymbol lhs, float scalar)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.multiply(lhs, scalar);

            return sym_np_ops.multiply(lhs, scalar);
        }

        public static NDArrayOrSymbol operator *(float scalar, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.multiply(rhs, scalar);

            return sym_np_ops.multiply(rhs, scalar);
        }

        public static NDArrayOrSymbol operator /(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.true_divide(lhs, rhs);

            return sym_np_ops.true_divide(lhs, rhs);
        }

        public static NDArrayOrSymbol operator /(NDArrayOrSymbol lhs, float scalar)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.true_divide(lhs, scalar);

            return sym_np_ops.true_divide(lhs, scalar);
        }

        public static NDArrayOrSymbol operator /(float scalar, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.true_divide(rhs, scalar);

            return sym_np_ops.true_divide(rhs, scalar);
        }

        public static NDArrayOrSymbol operator >(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.greater(lhs, rhs);

            return sym_np_ops.greater(lhs, rhs);
        }

        public static NDArrayOrSymbol operator >=(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.greater_equal(lhs, rhs);

            return sym_np_ops.greater_equal(lhs, rhs);
        }

        public static NDArrayOrSymbol operator >(NDArrayOrSymbol lhs, float rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.greater(lhs, rhs);

            return sym_np_ops.greater(lhs, rhs);
        }

        public static NDArrayOrSymbol operator >=(NDArrayOrSymbol lhs, float rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.greater_equal(lhs, rhs);

            return sym_np_ops.greater_equal(lhs, rhs);
        }

        public static NDArrayOrSymbol operator >(float lhs, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.greater(rhs, lhs);

            return sym_np_ops.greater(rhs, lhs);
        }

        public static NDArrayOrSymbol operator >=(float lhs, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.greater_equal(rhs, lhs);

            return sym_np_ops.greater_equal(rhs, lhs);
        }

        public static NDArrayOrSymbol operator <(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.less(lhs, rhs);

            return sym_np_ops.less(lhs, rhs);
        }

        public static NDArrayOrSymbol operator <=(NDArrayOrSymbol lhs, NDArrayOrSymbol rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.less_equal(lhs, rhs);

            return sym_np_ops.less_equal(lhs, rhs);
        }

        public static NDArrayOrSymbol operator <(NDArrayOrSymbol lhs, float rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.less(lhs, rhs);

            return sym_np_ops.less(lhs, rhs);
        }

        public static NDArrayOrSymbol operator <=(NDArrayOrSymbol lhs, float rhs)
        {
            if (lhs.IsNDArray)
                return nd_np_ops.less_equal(lhs, rhs);

            return sym_np_ops.less_equal(lhs, rhs);
        }

        public static NDArrayOrSymbol operator <(float lhs, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.less(rhs, lhs);

            return sym_np_ops.less(rhs, lhs);
        }

        public static NDArrayOrSymbol operator <=(float lhs, NDArrayOrSymbol rhs)
        {
            if (rhs.IsNDArray)
                return nd_np_ops.less_equal(rhs, lhs);

            return sym_np_ops.less_equal(rhs, lhs);
        }

        #endregion
    }

    public class NDArrayOrSymbolList : IEnumerable<NDArrayOrSymbol>
    {
        public List<NDArrayOrSymbol> data;

        public NDArrayOrSymbolList()
        {
            data = new List<NDArrayOrSymbol>();
        }

        public NDArrayOrSymbolList(params NDArrayOrSymbol[] args)
        {
            data = args.ToList();
        }

        public NDArrayOrSymbolList(params ndarray[] args)
        {
            data = new List<NDArrayOrSymbol>();
            foreach (var item in args)
            {
                data.Add(item);
            }
        }

        public NDArrayOrSymbolList(params _Symbol[] args)
        {
            data = new List<NDArrayOrSymbol>();
            foreach (var item in args)
            {
                data.Add(item);
            }
        }

        public NDArrayOrSymbolList((NDArrayOrSymbol, NDArrayOrSymbol) args)
        {
            data = new List<NDArrayOrSymbol> { args.Item1, args.Item2 };
        }

        public NDArrayOrSymbolList((NDArrayOrSymbol, NDArrayOrSymbol, NDArrayOrSymbol) args)
        {
            data = new List<NDArrayOrSymbol> { args.Item1, args.Item2, args.Item3 };
        }

        public NDArrayOrSymbol[] Data => data.ToArray();
        
        public NDArrayOrSymbol this[int i]
        {
            get => data[i];
            set => data[i] = value;
        }

        public int Length => data.Count;

        public IEnumerator<NDArrayOrSymbol> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public void Add(params NDArrayOrSymbol[] x)
        {
            if (x == null)
                return;

            data.AddRange(x);
        }

        public NDArrayList NDArrays
        {
            get
            {
                NDArrayList result = new NDArrayList();
                foreach (var item in data)
                {
                    if (item.IsNDArray)
                        result.Add(item);
                }
                return result;
            }
        }

        public SymbolList Symbols
        {
            get
            {
                SymbolList result = new SymbolList();
                foreach (var item in data)
                {
                    if (item.IsSymbol)
                        result.Add(item);
                }

                return result;
            }
        }

        public static implicit operator NDArrayOrSymbolList(NDArrayOrSymbol[] x)
        {
            return new NDArrayOrSymbolList(x);
        }

        public static implicit operator NDArrayOrSymbol[] (NDArrayOrSymbolList x)
        {
            return x.ToArray();
        }

        public static implicit operator NDArrayOrSymbolList(NDArrayOrSymbol x)
        {
            return new NDArrayOrSymbolList(x);
        }

        public static implicit operator NDArrayOrSymbolList(List<NDArrayOrSymbol> x)
        {
            return new NDArrayOrSymbolList(x.ToArray());
        }

        public static implicit operator NDArrayOrSymbolList(SymbolList x)
        {
            return new NDArrayOrSymbolList(x);
        }

        public static implicit operator NDArrayOrSymbolList(NDArrayList x)
        {
            return new NDArrayOrSymbolList(x);
        }

        public static implicit operator NDArrayOrSymbolList(ndarray[] x)
        {
            return new NDArrayOrSymbolList(x);
        }

        public static implicit operator NDArrayOrSymbolList(_Symbol[] x)
        {
            return new NDArrayOrSymbolList(x);
        }
    }
}