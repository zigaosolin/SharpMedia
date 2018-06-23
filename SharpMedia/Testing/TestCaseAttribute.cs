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

namespace SharpMedia.Testing
{

    /// <summary>
    /// A test case attribute. Can be applied to any method.
    /// </summary>
    /// <re
    [AttributeUsage(AttributeTargets.Method, Inherited=false)]
    public abstract class TestCaseAttribute : Attribute
    {
        #region Private Members
        string name;
        #endregion

        #region Properties

        /// <summary>
        /// The name of test.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion


    }

    /// <summary>
    /// The correctness test, method should throw when something goes wrong. It may also
    /// return bool that indicates if everything went ok.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CorrectnessTestAttribute : TestCaseAttribute
    {
    }

    /// <summary>
    /// Performance test, method returns float/TimeSpan that measures performance. If method returns void,
    /// measurments are made externally.
    /// </summary>
    /// <remarks>Performance measurements are written to special table for comparison. This will
    /// allow perforance increase/decrease over time and comparison over internet.</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PerformanceTestAttribute : TestCaseAttribute
    {
        #region Private Members
        uint testRepeat = 1;
        TimeSpan maximumSpan = TimeSpan.MaxValue;
        string groupTable;
        #endregion

        #region Public Members

        /// <summary>
        /// How many times to repeat the test.
        /// </summary>
        public uint TestRepeat
        {
            get { return testRepeat; }
            set { testRepeat = value; }
        }

        /// <summary>
        /// Maximum time span, if any of the test take longer, it will produce errors.
        /// </summary>
        public TimeSpan MaximumTimeSpan
        {
            get
            {
                return maximumSpan;
            }
            set
            {
                maximumSpan = value;
            }
        }

        /// <summary>
        /// The group table, if similiar tests are to be compared.
        /// </summary>
        public string GroupTable
        {
            get
            {
                return groupTable;
            }
            set
            {
                groupTable = value;
            }
        }

        #endregion

        /// <summary>
        /// Creates a performance test.
        /// </summary>
        public PerformanceTestAttribute()
        {
        }
    }


    /// <summary>
    /// A visual test requires the observer to grade it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class VisualTestAttribute : TestCaseAttribute
    {
        #region Private Members
        TimeSpan terminateAfter = TimeSpan.MaxValue;
        #endregion

        #region Properties

        /// <summary>
        /// Automatic termination for visual tests.
        /// </summary>
        public TimeSpan TerminateAfterSpan
        {
            get { return terminateAfter; }
            set { terminateAfter = value; }
        }

        #endregion

        public VisualTestAttribute()
        {
        }
    }


    /// <summary>
    /// A stress test is something that results in abnormal conditions.
    /// </summary>
    public class StressTestAttribute : CorrectnessTestAttribute
    {

        public StressTestAttribute()
        {
        }
    }
}
