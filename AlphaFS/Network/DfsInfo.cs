/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Network
{
   /// <summary>Contains information about a Distributed File System (DFS) root or link. This class cannot be inherited.
   /// <para>This structure contains the name, status, GUID, time-out, number of targets, and information about each target of the root or link.</para>
   /// </summary>
   [SerializableAttribute]
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
   public sealed class DfsInfo
   {
      #region Constructor

      /// <summary>Initializes a new instance of the <see cref="DfsInfo"/> class which acts as a wrapper for a DFS root or link target.</summary>
      public DfsInfo()
      {
      }

      /// <summary>Initializes a new instance of the <see cref="DfsInfo"/> class, which acts as a wrapper for a DFS root or link target.</summary>
      /// <param name="structure">An initialized <see cref="NativeMethods.DFS_INFO_4"/> instance.</param>
      internal DfsInfo(NativeMethods.DFS_INFO_4 structure)
      {
         Comment = structure.Comment;
         EntryPath = structure.EntryPath;
         State = structure.State;
         Timeout = structure.Timeout;
         Guid = structure.Guid;

         _storageInfoCollection = new List<DfsStorageInfo>();

         for (int i = 0; i < structure.NumberOfStorages; i++)
            _storageInfoCollection.Add(new DfsStorageInfo(Utils.PtrToStructure<NativeMethods.DFS_STORAGE_INFO>(i, structure.Storage)));
      }

      #endregion // Constructor

      #region Methods

      #region ToString

      /// <summary>Returns the Universal Naming Convention (UNC) path of the DFS root or link.</summary>
      /// <returns>A string that represents this instance.</returns>
      public override string ToString()
      {
         return EntryPath;
      }

      #endregion // ToString

      #endregion // Methods

      #region Properties

      #region DirectoryInfo

      private DirectoryInfo _directoryInfo;

      /// <summary>The <see cref="DirectoryInfo"/> instance of the DFS root or link.</summary>
      public DirectoryInfo DirectoryInfo
      {
         get
         {
            // Do not use ?? expression here.
            if (_directoryInfo == null)
               _directoryInfo = new DirectoryInfo(null, EntryPath, PathFormat.FullPath);

            return _directoryInfo;
         }
      }

      #endregion // DirectoryInfo

      #region Comment

      /// <summary>The comment of the DFS root or link.</summary>
      public string Comment { get; internal set; }

      #endregion // Comment

      #region EntryPath

      /// <summary>The Universal Naming Convention (UNC) path of the DFS root or link.</summary>
      public string EntryPath { get; internal set; }

      #endregion // EntryPath

      #region Guid

      /// <summary>Specifies the GUID of the DFS root or link.</summary>
      public Guid Guid { get; internal set; }

      #endregion // Guid

      #region StorageInfoCollection

      private readonly List<DfsStorageInfo> _storageInfoCollection;

      /// <summary>The collection of DFS targets of the DFS root or link.</summary>
      public IEnumerable<DfsStorageInfo> StorageInfoCollection
      {
         get { return _storageInfoCollection; }
      }

      #endregion // StorageInfoCollection

      #region State

      /// <summary>An <see cref="DfsVolumeStates"/> enum that specifies a set of bit flags that describe the DFS root or link.</summary>
      public DfsVolumeStates State { get; internal set; }

      //DfsVolumeStates flavorBits = (structure3.State & (DfsVolumeStates) DfsNamespaceFlavors.All);
      //If (flavorBits == DFS_VOLUME_FLAVOR_STANDALONE)     // Namespace is stand-alone DFS.
      //else if (flavorBits == DFS_VOLUME_FLAVOR_AD_BLOB)   // Namespace is AD Blob.
      //else StateBits = (Flavor & DFS_VOLUME_STATES)        // Unknown flavor.
      // StateBits can be one of the following: 
      //  (DFS_VOLUME_STATE_OK, DFS_VOLUME_STATE_INCONSISTENT, 
      //   DFS_VOLUME_STATE_OFFLINE or DFS_VOLUME_STATE_ONLINE)
      //State = flavorBits | structure3.State;

      #endregion // State
      
      #region Timeout

      /// <summary>Specifies the time-out, in seconds, of the DFS root or link.</summary>
      public ulong Timeout { get; internal set; }

      #endregion // Timeout

      #endregion // Properties
   }
}