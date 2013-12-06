using SalesApplication.Info.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SalesApplication.Info
{
    /// <summary>Class for providing application information.</summary>
    /// <remarks>The functionality of this class has evolved into more than was anticipated. TODO- refactor out Assembly/AppDomain logic into a new class</remarks>
    public sealed partial class ApplicationInfo : IApplicationInfo
    {
        /// <summary>Gets the file paths from file names.</summary>
        /// <param name="fileNames">The file names.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <returns></returns>
        public IEnumerable<string> GetFilePathsFromFileNames(IEnumerable<string> fileNames, IEnumerable<string> filePaths)
        {
            IEnumerable<string> toReturn = (from fileName in fileNames
                                            from filePath in filePaths
                                            where filePath.Contains(fileName)
                                            select filePath).Distinct();                                         
            return toReturn;
        }

        /// <summary>Gets the file names from paths.</summary>
        /// <param name="paths">The assembly paths.</param>
        /// <returns></returns>
        public IEnumerable<string> GetFileNamesFromPaths(IEnumerable<string> paths)
        {
            // retrieve sorted list assembly file names (with extension)
            IEnumerable<string> results = from path in paths 
                                          select Path.GetFileName(path);
            return results;
        }

        /// <summary>
        ///  Returns the paths of assemblies residing within the current AppDomain's base directory that are not loaded.
        /// </summary>
        public IEnumerable<string> GetAssemblyPathsNotLoadedButReferenced()
        {
            // get file paths for assemblies both loaded into the AppDomain and not loaded but referenced via .csproj reference in
            // visual studio's Solution Explorer... Note that the file paths for loaded/referenced assemblies will vary between
            // the application contexts in which they are invoked.
            IEnumerable<string> loadedAssemblyPaths      = GetLoadedAssemblyPaths();
            IEnumerable<string> referencedAssemblyPaths  = GetReferencedAssemblyPathsFromAppDomainBaseDirectory();

            // get the file names from these paths...
            IEnumerable<string> loadedAssemblyFileNames     = GetFileNamesFromPaths(loadedAssemblyPaths);
            IEnumerable<string> referencedAssemblyFileNames = GetFileNamesFromPaths(referencedAssemblyPaths);

            // determine which assembly files are referenced but not loaded, into the current AppDomain
            IEnumerable<string> notLoadedButReferencedAssemblyFileNames = referencedAssemblyFileNames.Except(loadedAssemblyFileNames);
            
            // now determine the absolute disk locations of the unloaded files...
            IEnumerable<string> notLoadedButReferencedFilePaths = GetFilePathsFromFileNames(notLoadedButReferencedAssemblyFileNames, referencedAssemblyPaths);

            // and return them
            return notLoadedButReferencedFilePaths;
        }

        /// <summary>
        ///  Returns the location paths of non-dynamic assemblies loaded into the current AppDomain
        /// </summary>
        /// <remarks>Location information for dynamic assemblies is not applicable</remarks>
        public IEnumerable<string> GetLoadedAssemblyPaths()
        {
            IEnumerable<Assembly> loadedAssemblies  = GetCurrentAppDomainAssemblies();
            IEnumerable<string> loadedAssemblyPaths = loadedAssemblies.Where(a => !a.IsDynamic).Select(a => a.Location).ToArray();
            return loadedAssemblyPaths;
        }

        /// <summary>
        /// Loads the assembly with the argument nameFilter residing within the current AppDomain's base directory and returns it.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        public Assembly GetReferencedAssemblyFromAppDomainBaseDirectory(string nameFilter = "*")
        {
            return Assembly.LoadFrom(GetReferencedAssemblyPathsFromAppDomainBaseDirectory(nameFilter).First());
        }

        /// <summary>
        /// Aggregates the assembly paths with the argument nameFilter existing in current domain's base directory and returns them.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        public IEnumerable<string> GetReferencedAssemblyPathsFromAppDomainBaseDirectory(string nameFilter = "*")
        {
            string assemblyNameFilter = string.Format("{0}.dll", nameFilter);
            var referencedAssemblyPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, assemblyNameFilter);
            return referencedAssemblyPaths;
        }

        /// <summary>
        /// Returns DirectoryInfo relating to the AppDomain's currently executing assembly's execution directory.
        /// </summary>
        public DirectoryInfo GetAssemblyExecutionDirectory()
        {
            DirectoryInfo assemblyExecutionDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
            return assemblyExecutionDirectory;
        }

        /// <summary>
        /// Loads the assembly with the argument nameFilter from within the AppDomain's currently executing assembly's execution directory  and returns it.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        /// <returns></returns>
        public Assembly GetAssemblyWithinExecutionDirectory(string nameFilter)
        {
            DirectoryInfo pathToLoadFrom = GetAssemblyExecutionDirectory();
            Assembly toReturn = Assembly.LoadFrom(pathToLoadFrom.FullName + "\\" + nameFilter);
            return toReturn;
        }

        /// <summary>Gets the Assembly with the argument nameFilter that exists within the application domain.</summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        public Assembly GetCurrentAppDomainAssembly(string nameFilter)
        {
            Assembly[] appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Assembly> results = GetCurrentAppDomainAssemblies(nameFilter);
            return results.First();
        }

        /// <summary>Gets the current application domain assemblies matching the optional nameFilter.</summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        public IEnumerable<Assembly> GetCurrentAppDomainAssemblies(string fullNameFilter = "")
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            // retrieve sorted list of assemblies
            IEnumerable<Assembly>  results = from assembly in currentDomain.GetAssemblies()
                                             orderby assembly.GetName().Name
                                             select assembly;

            if (fullNameFilter != String.Empty)
            {
                // filter by assembly name (if filter specified)
                results = from assembly in results
                          where assembly.FullName.Contains(fullNameFilter)
                          select assembly;
            } 

            return results;
        }

        /// <summary>
        /// Gets the AssemblyData for the current application domain assemblies matching the optional name filter.
        /// </summary>
        /// <param name="nameFilter">The name filter for the Assembly to return.</param>
        /// <returns>IEnumerable<AssemblyData></returns>
        public IEnumerable<AssemblyData> GetCurrentAppDomainAssemblyInformation(string nameFilter = "")
        {
            IEnumerable<Assembly> assemblys = GetCurrentAppDomainAssemblies(nameFilter);

            IList<AssemblyData> assemblyInfos = new List<AssemblyData>();

            foreach (Assembly a in assemblys)
                assemblyInfos.Add(new AssemblyData(a));

            return assemblyInfos;
        }
    }
}
