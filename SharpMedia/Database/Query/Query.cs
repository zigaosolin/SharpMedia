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
using SharpMedia.Database.Query.Expressions;
using SharpMedia.Testing;
using SharpMedia.Database.Indexing;

namespace SharpMedia.Database.Query
{
    /// <summary>
    /// Performs a custom join.
    /// </summary>
    public delegate T JoinDelegate<T, Selectable1, Selectable2>(Selectable1 sel1, Selectable2 sel2);

    /// <summary>
    /// A query class, can issue queries.
    /// </summary>
    public static class Query
    {
        #region Helpers


        static List<Selectable> FindInternal<Selectable, T>(List<Selectable> result, Node<object> table,
            IQueryExpression<Selectable, T> expression)
        {
            TypedStream<T> typedStream = table.Open<T>(OpenMode.Read);

            if (typedStream == null)
            {
                throw new TypedStreamNotFoundException(
                    string.Format("Typed stream of type {0} not found in '{1}'", 
                    typeof(T), table.Path));
            }

            // We first obtain index table.
            IndexTable index2 = table.IndexTable;
            StreamIndexTable index = index2 != null ? index2[typeof(T)] : null;

            QueryFilter filter = expression.Filter;

            // We make sure it is disposed correcly.
            using (typedStream)
            {
                uint[] locations = typedStream.ObjectLocations;

                for (int i = 0; i < locations.Length; i++)
                {
                    // We have object's location.
                    uint primary = locations[i];

                    T processingObject = default(T);

                    // 1) We now filter all objects, first by index.
                    if ((filter & QueryFilter.Index) != 0)
                    {
                        Dictionary<string, object> indexData = 
                            StreamIndexTable.IndexOfObject<T>(index, typedStream, primary, out processingObject);

                        // We now process through filter.
                        if (!expression.IsSatisfied(primary, indexData)) continue;
                    }

                    // 2) Now we try the alternative filtering, by object.
                    if ((filter & QueryFilter.Object) != 0)
                    {
                        // Force object loading, if not already by indexing.
                        if (!object.ReferenceEquals(processingObject, null))
                        {
                            processingObject = typedStream.Read(primary);
                        }

                        // We now process it.
                        if (!expression.IsSatisfied(processingObject)) continue;
                    }

                    // 3) We now select it.
                    result.Add(expression.Select(processingObject));
                }
            }

            // 4) After all object are loaded, we sort them
            if ((filter & QueryFilter.Sort) != 0)
            {
                expression.Sort(result);
            }

            return result;
        }


        #endregion

        #region Static Members

        /// <summary>
        /// Open a typed stream of type T and issues a query.
        /// </summary>
        /// <remarks>You supply node instead of typed stream so disposal is automatic and to enable
        /// indexing if available.</remarks>
        public static QueryResults<Selectable> FindAsync<Selectable, T>(Node<object> table, 
            IQueryExpression<Selectable, T> expression)
        {
            return new QueryResults<Selectable>(delegate(List<Selectable> result)
            {
                Query.FindInternal<Selectable, T>(result, table, expression);
            });
        }

        /// <summary>
        /// Opens a typed stream of type T and issues a query.
        /// </summary>
        /// <remarks>You supply node instead of typed stream so disposal is automatic and to enable
        /// indexing if available.</remarks>
        public static List<Selectable> Find<Selectable, T>(Node<object> table, 
            IQueryExpression<Selectable, T> expression)
        {
            List<Selectable> result = new List<Selectable>();
            FindInternal<Selectable, T>(result, table, expression);
            return result;
        }

        /// <summary>
        /// Opens a typed stream of type T and deletes all matched objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static QueryResults<Selectable> DeleteAsync<Selectable, T>(Node<object> table, 
            IQueryExpression<Selectable, T> expression)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static uint Delete<Selectable, T>(Node<object> table, 
            IQueryExpression<Selectable, T> expression)
        {
            return 0;
        }

        /// <summary>
        /// Performs a joint of two tables.
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<Selectable1, Selectable2>> Join<Selectable1, T1, Selectable2, T2, Key>(
            Node<object> table1, IJoinExpression<Selectable1, T1, Key> expression1,
            Node<object> table2, IJoinExpression<Selectable2, T2, Key> expression2)
                where Key : IComparable<Key>
        {
            return new List<KeyValuePair<Selectable1, Selectable2>>(); ;
        }

        /// <summary>
        /// Performs a joint of two tables.
        /// </summary>
        /// <returns></returns>
        public static QueryResults<KeyValuePair<Selectable1, Selectable2>> JoinAsync<Selectable1, T1, Selectable2, T2, Key>(
            Node<object> table1, IJoinExpression<Selectable1, T1, Key> expression1,
            Node<object> table2, IJoinExpression<Selectable2, T2, Key> expression2)
                where Key : IComparable<Key>
        {
            return null;
        }

        /// <summary>
        /// Performs a joint of two tables.
        /// </summary>
        /// <returns></returns>
        public static List<Joined> Join<Joined, Selectable1, T1, Selectable2, T2, Key>(
            Node<object> table1, IJoinExpression<Selectable1, T1, Key> expression1,
            Node<object> table2, IJoinExpression<Selectable2, T2, Key> expression2,
            JoinDelegate<Joined, Selectable1, Selectable2> customJoin)
                where Key : IComparable<Key>
        {
            return null;
        }

        /// <summary>
        /// Performs a joint of two tables.
        /// </summary>
        /// <returns></returns>
        public static QueryResults<Joined> JoinAsync<Joined, Selectable1, T1, Selectable2, T2, Key>(
            Node<object> table1, IJoinExpression<Selectable1, T1, Key> expression1,
            Node<object> table2, IJoinExpression<Selectable2, T2, Key> expression2,
            JoinDelegate<Joined, Selectable1, Selectable2> customJoin)
                where Key : IComparable<Key>
        {
            return null;
        }


        #endregion

    }


#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class QueryTest
    {
        [Serializable]
        public class User
        {
            string name;
            string emso;
            float height;
            bool male;
            string dates;

            [Indexed]
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public bool Male
            {
                get { return male; }
                set { male = value; }
            }

            public float Height
            {
                get { return height; }
                set { height = value; }
            }

            [Indexed]
            public string Emso
            {
                get { return emso; }
                set { emso = value; }
            }

            public string Dates
            {
                get { return dates; }
                set { dates = value; }
            }

            public User(string name, string emso, float height, bool male, string dates)
            {
                this.name = name;
                this.emso = emso;
                this.height = height;
            }

        }

        Node<User> PrepareUsers()
        {
            if (manager.Find("/Testing/QueryUsers") != null)
            {
                manager.Root.Delete("/Testing/QueryUsers");
            }

            Node<User> node = manager.Create<User>("/Testing/QueryUsers", StreamOptions.Indexed);
            node.Array = new User[] { 
                new User("Bojan", "1214234353251", 1.78f, true, "Andreja"),
                new User("Janez", "3443124134132", 1.33f, true, null),
                new User("Andreja", "3412432432432", 1.66f, false, "Bojan"),
                new User("Tadej", "2314413413241", 1.92f, true, "Branka"),
                new User("Branka", "1441414141412", 1.67f, false, "Tadej")
            };

            return node;
        }

        DatabaseManager manager = null;

        [CorrectnessTest]
        public void FindTest()
        {
            Node<User> node = PrepareUsers();

            List<string> usersWithB = 
                Query.Find(node, new DelegateExpression<string, User>(
                    // Indexing search (will always be invoked, also for object that are not indexed - in
                    // this case, data will be extracted from object deserialized).
                    delegate(uint index, Dictionary<string, object> indexed)
                    {
                        return (indexed["Name"] as string).Contains("B");
                    },

                    // Selection: select the user name
                    delegate(User user) { return user.Name; })
                );


            // We try to extract users with auto-selection.
            List<User> usersHigherThan170 =
                Query.Find(node, new AutoSelectDelegateExpression<User>(
                delegate(User user) { return user.Height >= 1.70f; }));
        }

        public class MaleFemale
        {
            public User Female;
            public User Male;
        }

        [CorrectnessTest]
        public void JoinTest()
        {
            Node<User> node = PrepareUsers();


            // We find all pairs of male-female.
            List<MaleFemale> maleFemale =
                Query.Join<MaleFemale, User, User, User, User, uint>(
                    node, new AutoSelectJoinDelegateExpression<User, uint>(
                        // We select all females.
                        delegate(User userx) { return userx.Male == false; },
                        // We assign them index 0 (this is necessary for all-to-all relation)
                        delegate(User user1) { return 0; }
                    ),
                    // We select all males
                    node, new AutoSelectJoinDelegateExpression<User, uint>(
                        // We select all males.
                        delegate(User usery) { return usery.Male == true; },
                        // We assign them index 0 (this is for all-to-all realtion)
                        delegate(User user2) { return 0; }
                    ),

                    // Composition of results, could also use KeyValuePair version
                    delegate(User female, User male) 
                    { 
                        MaleFemale x = new MaleFemale(); 
                        x.Male = male; 
                        x.Female = female; 
                        return x; 
                    }
                );
                     
   
            // We find who dates who.
            List<MaleFemale> result = Query.Join<MaleFemale, User, User, User, User, string>(
                node, new AutoSelectJoinDelegateExpression<User, string>(delegate(User user1) { return user1.Dates; }),
                node, new AutoSelectJoinDelegateExpression<User, string>(delegate(User user2) { return user2.Name; }),
                delegate(User female, User male)
                {
                    MaleFemale x = new MaleFemale();
                    x.Male = male;
                    x.Female = female;
                    return x;
                }
            );


        }

    }

#endif



}
