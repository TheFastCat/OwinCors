using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;

namespace SalesApplication.Info.Model
{
    /// <summary>A struct to hold Assembly metadata as strings</summary>
    public sealed class AssemblyData
    {
        /// <summary> A regular expression that matches a _git commit hash long ie "5424475175c6c2d3c4eec354ccf82fe28caab652" </summary>
        private readonly Regex _gitCommitHashLongRegex = new Regex("\\b([a-f0-9]{40})\\b");

        private readonly Regex _notStronglyNamedRegex = new Regex("PublicKeyToken=null");

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        public AssemblyData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyData"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public AssemblyData(Assembly assembly)
        {
            FullName       = assembly.FullName;
            Name           = assembly.GetName().Name;
            IsFullyTrusted = assembly.IsFullyTrusted;
            Version        = assembly.GetName().Version.ToString();
            IsDynamic      = assembly.IsDynamic;

            if (!IsDynamic)
            {
                Location                     = assembly.Location;
                AssemblyInformationalVersion = GetAssemblyInformationalVersion(assembly);
            }
        }

        /// <summary>
        /// Determines whether the argument string represents a git commit hash.
        /// </summary>
        /// <param name="toCheck">To check.</param>
        private bool IsGitCommitHash(string toCheck)
        {
            if (string.IsNullOrWhiteSpace(toCheck))
                return false;
            return _gitCommitHashLongRegex.Match(toCheck).Success;
        }
        /// <summary>
        /// Determines whether the assembly is strongly named.
        /// </summary>
        /// <param name="toCheck">To check.</param>
        private bool IsStronglyNamed(string toCheck)
        {
            if (string.IsNullOrWhiteSpace(toCheck))
                return false;
            return !_notStronglyNamedRegex.Match(toCheck).Success;
        }
        /// <summary>
        /// Returns the [AssemblyInformationalVersion] for the assembly as a string
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        /// <remarks>
        /// For assemblies consuming SalesApplication.Versioning's auto versioning this represents the
        /// git commit hash associated with the assembly's version
        /// </remarks>
        private string GetAssemblyInformationalVersion(Assembly assembly)
        {
            // dynamic assemblies do not support this operation
            if (!assembly.IsDynamic)
                return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
            else
                return String.Empty;
        }

        /// <summary>Gets or sets the full name.</summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }
        /// <summary>Gets or sets the location.</summary>
        /// <value>The location.</value>
        public string Location { get; set; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is fully trusted].
        /// </summary>
        /// <value><c>true</c> if [is fully trusted]; otherwise, <c>false</c>.</value>
        public bool IsFullyTrusted { get; set; }
        /// <summary>Gets or sets the version.</summary>
        /// <value>The version.</value>
        public string Version { get; set; }
        /// <summary>Gets or sets the assembly informational version.</summary>
        /// <value>The assembly informational version.</value>
        public string AssemblyInformationalVersion { get; set; }
        /// <summary>
        /// Gets a value indicating whether [assembly informational version is git commit hash].
        /// </summary>
        /// <value>
        /// <c>true</c> if [assembly informational version is git commit hash]; otherwise, <c>false</c>.
        /// </value>
        public bool AssemblyInformationalVersionIsGitCommitHash 
        {
            get
            {
                return IsGitCommitHash(AssemblyInformationalVersion);
            }
        }
        /// <summary>
        /// Gets a value indicating whether [assembly is strongly named].
        /// </summary>
        /// <value>
        /// <c>true</c> if [assembly is strongly name]; otherwise, <c>false</c>.
        /// </value>
        public bool AssemblyIsStronglyNamed
        {
            get
            {
                return IsStronglyNamed(FullName);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [is dynamic].
        /// </summary>
        /// <value><c>true</c> if [is dynamic]; otherwise, <c>false</c>.</value>
        public bool IsDynamic { get; set; }
    }
}
