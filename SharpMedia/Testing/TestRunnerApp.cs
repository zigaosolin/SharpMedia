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
using SharpMedia.Components.Applications;
using SharpMedia.Database;
using System.IO;
using SharpMedia.Components.Configuration;
using System.Xml;
using System.Reflection;
using System.Globalization;
using SharpMedia.Components.Configuration.ComponentProviders;

namespace SharpMedia.Testing
{
    /// <summary>
    /// The test run mode.
    /// </summary>
    public enum TestRunMode
    {
        /// <summary>
        /// Arguments are assemblies.
        /// </summary>
        Assemblies,

        /// <summary>
        /// Arguments are classes.
        /// </summary>
        Classes,

        /// <summary>
        /// First argument is test suite class, followed by methods
        /// to be run.
        /// </summary>
        TestSuite
    }

    /// <summary>
    /// A test runner application.
    /// </summary>
    [AutoParametrize]
    public sealed class TestRunnerApp : Application
    {
        #region Private Members
        IComponentDirectory componentDirectory;
        
        // Configuration.
        TestRunMode mode = TestRunMode.Assemblies;
        bool runCorrectnessTests = true;
        bool runStressTests = false;
        bool runPerformanceTests = true;
        bool runVisualTests = true;

        string reportNode;
        #endregion

        #region Properties

        [Required]
        public IComponentDirectory ComponentDirectory
        {
            get { return componentDirectory; }
            set { componentDirectory = value; }
        }

        /// <summary>
        /// The run mode of a test.
        /// </summary>
        public TestRunMode RunMode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        /// Should we run performance tests.
        /// </summary>
        public bool RunPerformanceTests
        {
            get { return runPerformanceTests; }
            set { runPerformanceTests = value; }
        }

        /// <summary>
        /// Should we run visual tests.
        /// </summary>
        public bool RunVisualTests
        {
            get { return runVisualTests; }
            set { runVisualTests = value; }
        }

        /// <summary>
        /// Should we run correctness tests.
        /// </summary>
        public bool RunCorrectnessTests
        {
            get { return runCorrectnessTests; }
            set { runCorrectnessTests = value; }
        }

        /// <summary>
        /// Should we run stress tests.
        /// </summary>
        public bool RunStressTests
        {
            get { return runStressTests; }
            set { runStressTests = value; }
        }

        /// <summary>
        /// Where should we print report (an XML document).
        /// </summary>
        public string ReportNode
        {
            get { return reportNode; }
            set { reportNode = value; }
        }

        #endregion

        #region Helpers

        void OutputError(string format, params object[] p)
        {
            console.Error.WriteLine(format, p);
        }

        void OutputLog(string format, params object[] p)
        {
            console.Out.WriteLine(format, p);
        }

        #endregion

        #region Overrides

        string FormatTime(TimeSpan span)
        {
            return span.TotalSeconds.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        XmlElement CreateExceptionReport(XmlDocument document, Exception ex)
        {
            XmlElement errorDesc = document.CreateElement("ExceptionError");

            // We now fill in the description.
            XmlAttribute exType = document.CreateAttribute("ExceptionType");
            XmlAttribute message = document.CreateAttribute("Message");
            XmlAttribute stackTrace = document.CreateAttribute("StackTrace");
            XmlAttribute source = document.CreateAttribute("Source");
            XmlAttribute link = document.CreateAttribute("Link");

            errorDesc.Attributes.Append(exType);
            errorDesc.Attributes.Append(message);
            errorDesc.Attributes.Append(stackTrace);
            errorDesc.Attributes.Append(source);
            errorDesc.Attributes.Append(link);

            exType.Value = ex.GetType().FullName;
            message.Value = ex.Message;
            stackTrace.Value = ex.StackTrace;
            source.Value = ex.Source;
            link.Value = ex.HelpLink;

            return errorDesc;
        }

        void RunTest(MethodInfo method, XmlDocument document, XmlNode testSuite, string testSuiteName, object testSuiteInstance)
        {
            object[] methodAttributes = method.GetCustomAttributes(false);
            TestCaseAttribute testAttribute = null;

            // We check if it is a test.
            Type methodTestType = null;
            for (int i = 0; i < methodAttributes.Length; i++)
            {
                if (methodAttributes[i] is TestCaseAttribute)
                {
                    if (methodTestType != null)
                    {
                        OutputError("Test {0} has more than one test attributes, ignoring all but first.");
                        continue;
                    }

                    methodTestType = methodAttributes[i].GetType();
                    testAttribute = methodAttributes[i] as TestCaseAttribute;
                }
            }

            if (methodTestType == null) return;

            // We construct test.
            XmlNode testNode;
            if (methodTestType == typeof(CorrectnessTestAttribute))
            {
                testNode = document.CreateElement("CorrectnessTest");
            }
            else if (methodTestType == typeof(PerformanceTestAttribute))
            {
                testNode = document.CreateElement("PerformanceTest");
            }
            else if (methodTestType == typeof(VisualTestAttribute))
            {
                testNode = document.CreateElement("VisualTest");
            }
            else if (methodTestType == typeof(StressTestAttribute))
            {
                testNode = document.CreateElement("StressTest");
            }
            else
            {
                testNode = document.CreateElement("UnknownTest");
            }

            // Now we construct test name.
            XmlAttribute testName = document.CreateAttribute("Name");
            testName.Value = method.Name;
            testNode.Attributes.Append(testName);

            // Now we have a valid test run.
            if (method.GetParameters().Length != 0)
            {
                OutputError("Test {0}.{1} has non-empty parameter list, cannot run.");

                XmlAttribute errorNode = document.CreateAttribute("Error");
                errorNode.Value = "Invalid parameter list (non-null)";
                testNode.Attributes.Append(errorNode);

                return;
            }

            // We can run it now.
            if (methodTestType == typeof(CorrectnessTestAttribute) || methodTestType == typeof(StressTestAttribute))
            {
                if (methodTestType == typeof(StressTestAttribute))
                {
                    if (!runCorrectnessTests)
                    {
                        // Test run is supressed.
                        XmlAttribute supressTest = document.CreateAttribute("Supressed");
                        supressTest.Value = "true";
                        testNode.Attributes.Append(supressTest);
                        return;
                    }
                }
                else
                {
                    if (!runCorrectnessTests)
                    {
                        // Test run is supressed.
                        XmlAttribute supressTest = document.CreateAttribute("Supressed");
                        supressTest.Value = "true";
                        testNode.Attributes.Append(supressTest);
                        return;
                    }
                }


                // We now run it
                try
                {
                    OutputLog("Running test '{0}.{1}'", testSuiteInstance, method.Name);
                    object obj = method.Invoke(testSuiteInstance, null);

                    if (method.ReturnType == typeof(bool))
                    {
                        if ((bool)obj == false)
                        {
                            XmlElement errorDesc = document.CreateElement("ReturnFalseError");
                            testNode.AppendChild(errorDesc);
                        }
                    }
                    else if (method.ReturnType != typeof(void))
                    {
                        OutputError("Test {0}.{1} does return but only valid returns" +
                            "are bool or void, returned {2}", testSuite, method.Name, obj == null ? "null" : obj.ToString());

                    }
                }
                catch (Exception ex)
                {
                    OutputError("Test {0} failed because of exception {1}", testName, ex);
                    testNode.AppendChild(CreateExceptionReport(document, ex));
                }
            }
            else if (methodTestType == typeof(PerformanceTestAttribute))
            {
                // Performance test.
                if (!runPerformanceTests)
                {
                    // Test run is supressed.
                    XmlAttribute supressTest = document.CreateAttribute("Supressed");
                    supressTest.Value = "true";
                    testNode.Attributes.Append(supressTest);
                    return;
                }

                PerformanceTestAttribute performanceAttribute =
                        testAttribute as PerformanceTestAttribute;

                for (int i = 0; i < performanceAttribute.TestRepeat; i++)
                {
                    XmlElement testRun = document.CreateElement("TestRun");

                    // We append if.
                    XmlAttribute runId = document.CreateAttribute("Id");
                    runId.Value = i.ToString();
                    testRun.Attributes.Append(runId);

                    testNode.AppendChild(testRun);

                    // We now run it
                    try
                    {
                        // We measure.
                        DateTime start = DateTime.Now;
                        object obj = method.Invoke(testSuiteInstance, null);
                        DateTime end = DateTime.Now;

                        // We have time span.
                        TimeSpan span = start - end;

                        // We always append overall time.
                        XmlAttribute overAllTime = document.CreateAttribute("OverallTime");
                        overAllTime.Value = FormatTime(span);
                        testRun.Attributes.Append(overAllTime);

                        if (method.ReturnType == typeof(float))
                        {
                            span = new TimeSpan((long)((float)obj * 10e-7f));
                        }
                        else if (method.ReturnType == typeof(double))
                        {
                            span = new TimeSpan((long)((double)obj * 10e-7));
                        }
                        else if (method.ReturnType == typeof(TimeSpan))
                        {
                            span = (TimeSpan)obj;
                        }
                        else if (method.ReturnType != typeof(void))
                        {
                            OutputError("Test {0}.{1} does return but only valid returns" +
                                "are float, double, TimeSpan or void, returned {2}", testSuiteName, method.Name, obj == null ? "null" : obj.ToString());
                        }

                        // We now write to report.
                        XmlAttribute time = document.CreateAttribute("Time");
                        time.Value = FormatTime(span);
                        testRun.Attributes.Append(time);

                    }
                    catch (Exception ex)
                    {
                        OutputError("Test '{0}.{1}' failed because of exception {2}", testSuiteName, method.Name, ex);
                        testRun.AppendChild(CreateExceptionReport(document, ex));
                    }
                }
            }
            else if (methodTestType == typeof(VisualTestAttribute))
            {
                if (!runVisualTests)
                {

                }

                OutputError("Not supported test: visual test");
                
            }
            else
            {
                OutputError("Unknown test type");
            }

            testSuite.AppendChild(testNode);
        }

        public override int Start(string verb, string[] arguments)
        {

            XmlDocument document = new XmlDocument();
            XmlNode results = document.CreateElement("TestRunResults");
            document.AppendChild(results);

            try
            {
                if (mode == TestRunMode.Classes)
                {
                    foreach (string testSuite in arguments)
                    {
                        // Creates a test suite element.
                        XmlNode testSuiteNode = document.CreateElement("TestSuite");
                        results.AppendChild(testSuiteNode);

                        // The type.
                        XmlAttribute attType = document.CreateAttribute("Type");
                        attType.Value = testSuite;
                        testSuiteNode.Attributes.Append(attType);

                        // We check for correctness.
                        Type type = Type.GetType(testSuite);
                        if (type == null || type.GetCustomAttributes(typeof(TestSuiteAttribute), true).Length == 0)
                        {
                            OutputError("Test suite {0} does not exist or is not a test suite (not [TestSuite] attribute)", testSuite);

                            // We signal attribute.
                            XmlAttribute att = document.CreateAttribute("Error");
                            att.Value = "Test suite does not exist or is not marked with TestSuiteAttribute";
                            testSuiteNode.Attributes.Append(att);
                            continue;
                        }

                        // We now obtain an instance of test suite through configuration.
                        object testSuiteInstance =
                            componentDirectory.ConfigureInlineComponent(
                            new ConfiguredComponent(type.FullName));

                        // We now call all tests.
                        foreach (MethodInfo method in
                            type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                        {
                            RunTest(method, document, testSuiteNode, testSuite, testSuiteInstance);  
                        }
                    }

                }
                else if (mode == TestRunMode.Assemblies)
                {
                    foreach (string asmName in arguments)
                    {
                        // Creates a test suite element.
                        XmlNode assemblyNode = document.CreateElement("Assembly");
                        results.AppendChild(assemblyNode);

                        // The type.
                        XmlAttribute attFullName = document.CreateAttribute("FullName");
                        attFullName.Value = asmName;
                        assemblyNode.Attributes.Append(attFullName);

                        Assembly asm = Assembly.Load(new AssemblyName(asmName));
                        if (asm == null)
                        {
                            OutputError("Assembly {0} failed to load", asmName);

                            // We signal attribute.
                            XmlAttribute att = document.CreateAttribute("Error");
                            att.Value = "Assembly failed to load";
                            assemblyNode.Attributes.Append(att);
                            continue;
                        }
                        // check all types that all TSd
                        foreach (Type type in asm.GetTypes())
                        {
                            if (type.GetCustomAttributes(typeof(TestSuiteAttribute), true).Length == 0) continue;

                            // Creates a test suite element.
                            XmlNode testSuiteNode = document.CreateElement("TestSuite");
                            assemblyNode.AppendChild(testSuiteNode);

                            // The type.
                            XmlAttribute attType = document.CreateAttribute("Type");
                            attType.Value = type.FullName;
                            testSuiteNode.Attributes.Append(attType);

                            // We now obtain an instance of test suite through configuration.
                            object testSuiteInstance =
                                componentDirectory.ConfigureInlineComponent(
                                new ConfiguredComponent(type.FullName));

                            // We now call all tests.
                            foreach (MethodInfo method in
                                type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                            {
                                RunTest(method, document, testSuiteNode, type.FullName, testSuiteInstance);
                            }
                        }
                    }
                }
                else
                {

                }

            }
            finally
            {
                // We serialize report.
                document.Save(console.Out);
                // to the node
                if (reportNode != null)
                {
                    Node<object> node = this.database.Find<object>(reportNode);
                    if (node == null)
                    {
                        OutputError("The report node path {0} does not exist, could not write the report", reportNode);
                    }
                    else
                    {
                    }
                }
            }
            return 0;
        }

        #endregion
    }
}
