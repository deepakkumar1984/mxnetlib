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
using MxNet.Interop;

// ReSharper disable once CheckNamespace
namespace MxNet
{
    public sealed class NDBlob : DisposableMXNetObject
    {
        #region Properties

        public IntPtr Handle => NativePtr;

        #endregion

        #region Methods

        #region Overrides

        protected override void DisposeUnmanaged()
        {
            Dispose();
            NativeMethods.MXNDArrayFree(NativePtr);
        }

        #endregion

        #endregion

        #region Constructors

        public NDBlob()
            : this(IntPtr.Zero)
        {
        }

        public NDBlob(IntPtr handle)
        {
            NativePtr = handle;
        }

        ~NDBlob()
        {
            DisposeUnmanaged();
        }

        #endregion
    }
}