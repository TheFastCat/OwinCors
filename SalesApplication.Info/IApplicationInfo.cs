using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SalesApplication.Info.Model;
using System.IO;

namespace SalesApplication.Info
{
    public interface IApplicationInfo
    {
        /// <summary>Gets the file paths from file names.</summary>
        /// <param name="fileNames">The file names.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <returns></returns>
        IEnumerable<string> GetFilePathsFromFileNames(IEnumerable<string> fileNames, IEnumerable<string> filePaths);

        /// <summary>Gets the assembly file names from paths.</summary>
        /// <param name="paths">The assembly paths.</param>
        /// <returns></returns>
        IEnumerable<string> GetFileNamesFromPaths(IEnumerable<string> paths);

        /// <summary>Gets the assembly paths not loaded but referenced.</summary>
        /// <returns></returns>
        IEnumerable<string> GetAssemblyPathsNotLoadedButReferenced();

        /// <summary>Gets the loaded assembly paths.</summary>
        /// <returns></returns>
        IEnumerable<string> GetLoadedAssemblyPaths();

        /// <summary>
        /// Loads the assembly with the argument nameFilter residing within the current AppDomain's base directory and returns it.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        Assembly GetReferencedAssemblyFromAppDomainBaseDirectory(string nameFilter);

        /// <summary>
        /// Aggregates the assembly paths with the argument nameFilter existing in current domain's base directory and returns them.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        IEnumerable<string> GetReferencedAssemblyPathsFromAppDomainBaseDirectory(string nameFilter = "*");

        /// <summary>
        /// Loads the assembly with the argument nameFilter within the current AppDomain's base directory and returns it.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        DirectoryInfo GetAssemblyExecutionDirectory();

        /// <summary>
        /// Loads the assembly with the argument nameFilter from within the AppDomain's currently executing assembly's execution directory  and returns it.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        /// <returns></returns>
        Assembly GetAssemblyWithinExecutionDirectory(string nameFilter);

        /// <summary>Gets the Assembly with the argument nameFilter that exists within the application domain.</summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        Assembly GetCurrentAppDomainAssembly(string nameFilter);

        /// <summary>Gets the current application domain assemblies matching the optional nameFilter.</summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        IEnumerable<Assembly> GetCurrentAppDomainAssemblies(string nameFilter = "");

        /// <summary>Gets a IEnumerable<AssemblyInfo> representing the current application domain assemblies matching the optional name filter.</summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        /// <returns>IEnumerable<AssemblyInfo></returns>
        IEnumerable<AssemblyData> GetCurrentAppDomainAssemblyInformation(string nameFilter = "");
    }
}
