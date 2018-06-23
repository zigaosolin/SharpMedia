// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team
// and is licensed for your use under the conditions of the NDA or other legally binding contract
// that you or a legal entity you represent has signed with the SharpMedia team.
// In an event that you have received or obtained this file without such legally binding contract
// in place, you MUST destroy all files and other content to which this lincese applies and
// contact the SharpMedia team for further instructions at the internet mail address:
//
//    legal@sharpmedia.com
//
using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Testing;
using System.Threading;
using SharpMedia.Database.Managed;
using SharpMedia.Database.Aspects;

namespace SharpMedia.Database
{
#if SHARPMEDIA_TESTSUITE

    /// <summary>
    /// Database node tester.
    /// </summary>
    public abstract class NodeTest
    {
        /// <summary>
        /// The method that DB must provide in order to support node testing. Constructor
        /// should create DB and garantie node.
        /// </summary>
        protected abstract Node<object> RootNode
        {
            get;
        }

        private INode GetRootNode()
        {
            return RootNode.AsINode();
        }


        [CorrectnessTest]
        public void CreateChild()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest1", typeof(string), 0);
            Assert.IsNotNull(node);
        }

        [CorrectnessTest]
        public void CreateAndGetName()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest2", typeof(string), 0);
            Assert.AreEqual(node.Name, "CreateChildTest2");
        }

        [CorrectnessTest]
        public void Rename()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest3", typeof(string), 0);
            node.Name = "CreateChildTest_3";
            Assert.AreEqual(node.Name, "CreateChildTest_3");
        }

        [CorrectnessTest]
        public void GetDefaultType()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest4", typeof(string), 0);
            Assert.AreEqual(node.DefaultType, typeof(string));
        }

        [CorrectnessTest]
        public void DefaultStreamQuery()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest5", typeof(string), 0);
            ITypedStream s = node.OpenDefaultStream(OpenMode.ReadWrite);
            Assert.IsNotNull(s);
        }

        [CorrectnessTest]
        public void ChangeDefaultStream()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest0", typeof(string), 0);
            node.AddTypedStream(typeof(NodeTest), 0);
            node.ChangeDefaultStream(typeof(NodeTest));
            Assert.AreEqual(node.DefaultType, typeof(NodeTest));
        }

        [CorrectnessTest]
        public void AddTypedStream()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest6", typeof(string), 0);
            node.AddTypedStream(typeof(NodeTest), 0);
            Assert.IsNotNull(node.GetTypedStream(OpenMode.ReadWrite, typeof(NodeTest)));
        }

        [CorrectnessTest]
        public void Link()
        {
            Assert.IsTrue(true);
        }

        [CorrectnessTest]
        public void CreateNewVersion1()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest7", typeof(string), 0);
            Assert.IsNotNull(node.CreateNewVersion(typeof(string), StreamOptions.None));
        }

        [CorrectnessTest]
        public void CreateNewVersion2()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest8", typeof(string), 0);
            Assert.IsNotNull(node.CreateNewVersion(typeof(object), StreamOptions.Compressed));
        }

        [CorrectnessTest]
        public void ObtainPreviousVersion()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest9", typeof(string), 0);
            INode node2 = node.CreateNewVersion(typeof(string), StreamOptions.Interleaved);
            Assert.IsNotNull(node2.PreviousVersion);
        }

        [CorrectnessTest]
        public void NullPreviousVersion()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest100", typeof(string), 0);
            Assert.IsNull(node.PreviousVersion);  
        }

        [CorrectnessTest]
        public void DeepChildCreation()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest110", typeof(string), 0);
            GetRootNode().CreateChild("CreateChildTest110/X", typeof(string), 0);

            Assert.IsNotNull(node.Find("X"));
        }

        [CorrectnessTest]
        public void CreateX()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest101", typeof(string), 0);
            foreach (Type t in node.TypedStreams)
            {
                Assert.IsNotNull(t);
            }
        }

        [CorrectnessTest]
        public void FindInHierarchy()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest10", typeof(string), 0);
            Assert.AreEqual(GetRootNode().Find("CreateChildTest10").Name, node.Name);
        }

        [CorrectnessTest]
        public void DeleteNode()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest11", typeof(string), 0);
            GetRootNode().DeleteChild(node.Name);
            Assert.IsNull(GetRootNode().Find("CreateChildTest11"));
        }

        [CorrectnessTest]
        public void DeleteNode2()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest12", typeof(string), 0);
            GetRootNode().DeleteChild("CreateChildTest12");
            Assert.IsNull(GetRootNode().Find("CreateChildTest12"));
        }

        [CorrectnessTest]
        public void AdoptChild()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest13", typeof(string), 0);
            GetRootNode().DeleteChild("CreateChildTest13");
            Assert.IsNull(GetRootNode().Find("CreateChildTest13"));
        }

        [CorrectnessTest]
        public void ListStreams()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest14", typeof(string), 0);
            Assert.AreEqual(node.TypedStreams.Length, 1);
        }

        [CorrectnessTest]
        public void ListChildren()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest15", typeof(string), 0);
            Assert.AreEqual(node.Children.Count, 0);
        }

        [CorrectnessTest]
        public void TimeStamps()
        {
            Assert.IsTrue(true);
        }


        [CorrectnessTest]
        public void MTAccess()
        {
            INode node = GetRootNode().CreateChild("CreateChildTest16", typeof(string), 0);

            // We access from two threads.
            bool caught = false;
            
            ParameterizedThreadStart ts = new ParameterizedThreadStart(delegate(object obj) {
                try
                {
                    ITypedStream stream = node.OpenDefaultStream(OpenMode.Write);
                    Thread.Sleep(1000);
                    stream.Dispose();
                }
                catch (MultiWriteAccessException )
                {
                    // Must happen.
                    caught = true;
                }
            });

            Thread t1 = new Thread(ts), t2 = new Thread(ts);
            t1.Start(node);
            t2.Start(node);
            t1.Join();
            t2.Join();

            Assert.IsTrue(caught);
        }

        [CorrectnessTest]
        public void BaseNode()
        {
            INode node1 = GetRootNode().CreateChild("CreateChildTest17_1", typeof(string), StreamOptions.None);
            INode node2 = GetRootNode().CreateChild("CreateChildTest18_2", typeof(object), StreamOptions.AllowDerivedTypes);
            node2.Base = node1;
            using (ITypedStream s2 = node1.OpenDefaultStream(OpenMode.Write))
            {
                s2.Write(0, "myname");
            }

            // We can access it through node 2.
            using (ITypedStream s = node2.GetTypedStream(OpenMode.Read, typeof(string)))
            {
                Assert.AreEqual((string)s.Read(0), "myname");
            }
        }

        [CorrectnessTest]
        public void Copy()
        {
            INode root = GetRootNode().CreateChild("CopyTest", typeof(object), StreamOptions.None);

            INode child = root.CreateChild("Child1", typeof(string), StreamOptions.SingleObject);
            using (ITypedStream ts = child.OpenDefaultStream(OpenMode.Write))
            {
                ts.Write(0, "my data");
            }

            root = GetRootNode().CreateChild("CopyTest_2", typeof(object), StreamOptions.None);
            child = child.CopyTo(root);

            Assert.AreEqual(typeof(string), child.DefaultType);
            using (ITypedStream ts = child.OpenDefaultStream(OpenMode.Read))
            {
                Assert.AreEqual("my data", ts.Read(0) as string);
            }
        }

        [CorrectnessTest]
        public void Move()
        {
            INode root = GetRootNode().CreateChild("CopyTest", typeof(object), StreamOptions.None);

            INode child = root.CreateChild("Child1", typeof(string), StreamOptions.SingleObject);
            using (ITypedStream ts = child.OpenDefaultStream(OpenMode.Write))
            {
                ts.Write(0, "my data");
            }

            root = GetRootNode().CreateChild("CopyTest_2", typeof(object), StreamOptions.None);
            child = child.MoveTo(root);

            Assert.AreEqual(typeof(string), child.DefaultType);
            using (ITypedStream ts = child.OpenDefaultStream(OpenMode.Read))
            {
                Assert.AreEqual("my data", ts.Read(0) as string);
            }
        }

        [CorrectnessTest]
        public void FindTest()
        {
            INode n = GetRootNode();
            n = n.Find("/");
        }

        [CorrectnessTest]
        public void AllocsDeallocs()
        {
            INode n = GetRootNode();
            n = n.CreateChild("CreateChildTest_20", typeof(string), StreamOptions.None);

            // Create big data.
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                builder.Append("Some data.\n");// 24 kBytes
            }

            // Write 2, delete 2 and write two.
            ITypedStream stream = n.OpenDefaultStream(OpenMode.ReadWrite);
            stream.Write(0, builder.ToString());
            stream.Write(1, builder.ToString());
            Assert.IsNotNull(stream.Read(0));
            Assert.IsNotNull(stream.Read(1));
            stream.Erase(1, 1, true);
            stream.Write(2, builder.ToString());
            Assert.IsNotNull(stream.Read(2));
        }


        uint count = 0;
        ITypedStream CreateStream(Type defType, StreamOptions flags)
        {
            count++;
            INode node = GetRootNode().CreateChild("child_" + count.ToString(), defType, flags);
            return node.OpenDefaultStream(OpenMode.ReadWrite);
        }

        [CorrectnessTest]
        public void GetFlags()
        {
            ITypedStream stream = CreateStream(typeof(string), StreamOptions.Compressed);
            Assert.AreEqual(stream.Flags, StreamOptions.Compressed);
        }

        [CorrectnessTest]
        public void ReadWrite()
        {
            ITypedStream stream = CreateStream(typeof(string), 0);
            string s1 = "my monkey";
            stream.Write(0, s1);
            Assert.IsFalse(Object.ReferenceEquals(stream.Read(0), s1));
            Assert.AreEqual(s1, (string)stream.Read(0));
        }

        [CorrectnessTest]
        public void ReadWriteN()
        {
            ITypedStream stream = CreateStream(typeof(string), 0);
            string[] list = new string[] { "x", "y", "z", "f" };
            stream.WriteObjects(0, list);
            Assert.AreEqual(stream.Read(0, 4).Length, 4);
        }

        [CorrectnessTest]
        public void InsertBefore()
        {
            ITypedStream stream = CreateStream(typeof(string), 0);
            string s1 = "my monkey", s2 = "my monkey2";
            stream.Write(0, s1);
            stream.InsertBefore(0, s2);
            Assert.AreEqual((string)stream.Read(0), s2);
        }

        [CorrectnessTest]
        public void InsertAfter()
        {
            ITypedStream stream = CreateStream(typeof(string), 0);
            string s1 = "my monkey", s2 = "my monkey2";
            stream.Write(0, s1);
            stream.Write(1, s2);
            stream.InsertAfter(0, s2);
            Assert.AreEqual((string)stream.Read(1), s2);
        }

        [CorrectnessTest]
        public void ObjectType()
        {
            ITypedStream stream = CreateStream(typeof(string), StreamOptions.AllowDerivedTypes);
            string s1 = "my monkey", s2 = "my monkey2";
            stream.Write(0, s1);
            stream.Write(1, s2);
            Assert.AreEqual(stream.GetObjectType(0), typeof(string));
        }

        [CorrectnessTest]
        public void Erase()
        {
            ITypedStream stream = CreateStream(typeof(string), StreamOptions.AllowDerivedTypes);
            string s1 = "my monkey", s2 = "my monkey2";
            stream.Write(0, s1);
            stream.Write(1, s2);
            stream.Erase(0, 2, true);
            Assert.IsNull(stream.Read(0));
        }

    }

#endif
}
