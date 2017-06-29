﻿// Copyright (c) 2017, Columbia University 
// All rights reserved. 
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of Columbia University nor the names of its 
//    contributors may be used to endorse or promote products derived from 
//    this software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 
// 
// =============================================================
// Authors: 
// Carmine Elvezio, Mengu Sukan, Steven Feiner
// =============================================================
// 
// 

using System;
using System.Collections.Generic;
using MercuryMessaging.Task;
using UnityEngine.Networking;

namespace MercuryMessaging.Message
{
    /// <summary>
    /// MmMessage that sends a serialized object as payload
    /// </summary>
    public class MmMessageSerializable : MmMessage
    {
        /// <summary>
        /// Dictionary enumerating supported serialized types
        /// </summary>
        public static readonly Dictionary<int, Type> SerializableTypes = new Dictionary<int, Type>();

        /// <summary>
        /// Type of payload
        /// </summary>
        public int SerializableType;

        /// <summary>
        /// Serialized item Payload
        /// Item needs to implement IMmSerializable
        /// </summary>
        public IMmSerializable value;

        /// <summary>
        /// Creates a basic MmMessageSerializable
        /// </summary>
        public MmMessageSerializable()
        {}

        /// <summary>
        /// Creates a basic MmMessageSerializable, with a control block
        /// </summary>
        /// <param name="metadataBlock">Object defining the routing of messages</param>
        public MmMessageSerializable(MmMetadataBlock metadataBlock = null)
            : base (metadataBlock)
        {
        }

        /// <summary>
        /// Create an MmMessage, with control block, MmMethod, and an int
        /// </summary>
        /// <param name="iVal">Serializable Payload</param>
        /// <param name="mmMethod">Identifier of target MmMethod</param>
        /// <param name="metadataBlock">Object defining the routing of messages</param>
        public MmMessageSerializable(IMmSerializable iVal,
            MmMethod mmMethod = default(MmMethod),
            MmMetadataBlock metadataBlock = null)
            : base(mmMethod, metadataBlock)
        {
            value = iVal;
        }

        /// <summary>
        /// Duplicate an MmMessageSerializable
        /// </summary>
        /// <param name="message">Item to duplicate</param>
        public MmMessageSerializable(MmMessageSerializable message) : base(message)
        {}

        /// <summary>
        /// Message copy method
        /// </summary>
        /// <returns>Duplicate of MmMessage</returns>
        public override MmMessage Copy()
        {
            MmMessageSerializable newMessage = new MmMessageSerializable (this);

            newMessage.value = value.Copy();

            return newMessage;
        }

        /// <summary>
        /// Deserialize the message
        /// </summary>
        /// <param name="reader">UNET based deserializer object</param>
        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize (reader);
            SerializableType = reader.ReadInt32();

            value = (IMmSerializable)Activator.CreateInstance(SerializableTypes[SerializableType]);

            value.Deserialize(reader);
        }

        /// <summary>
        /// Serialize the MmMessage
        /// </summary>
        /// <param name="writer">UNET based serializer</param>
        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize (writer);
            writer.Write(SerializableType);
            value.Serialize (writer);
        }

    }
}
