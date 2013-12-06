  ///////////////////////////////////////////////////////////////
 //// SalesApplication.Versioning OVERVIEW   version 0.0.10 ////
///////////////////////////////////////////////////////////////

		(For an overview of what assembly versioning is see the 'README.txt' at the base level of this .sln)

		   ///////////
	 !!!! /IMPORTANT/ !!!!
		 ///////////

		DO NOT UPGRADE MSBuildTasks nuget package beyond the current version of:

			<package id="MSBuildTasks" version="1.4.0.61" targetFramework="net45" />

		The current version contains fixes to a defect that Reid implemented and has yet to be deployed to the MSBuildTasks nuget package.
		Upgrading the nuget package to a later version will break the build.

	  ////////////////////////////////////
	 /SalesApplication.Versioning.csproj/
	////////////////////////////////////

	Each time this project, Application.Versioning.csproj, is built, MSBUILD tasks defined in the 3rd-party, Nuget-installed 'MSBuildCommunity Tasks' library 
	dynamically generate [AssemblyFileVersion], [AssemblyInformationalVersion], [AssemblyVersion] based off of the current UTC date time and input them into a new file,
	AutoVersion.cs, that resides in this project's 'Properties' directory (alongside the AssemblyInfo.cs file). The existing AssemblyInfo.cs has these assembly attributes 
	forever culled to avoid build errors of multiply-defined assembly attributes). After generation of AutoVersion.cs (which happens just before build), the compiler
	integrates the aforementioned assembly versioning attributes into the built assembly. This versioning reflects the UTC time at which they were built as well as the 
	git commit hash (stored as [AssemblyInformationalVersion]) in order to allow tracking to the git commit from which the version was generated.

	You can see the logic that MSBUILD uses within the SalesApplication.Versioning.csproj file. To view it through VS, right-click and unload it from the .sln. Then right-click
	and edit it. The assembly versioning for all subscribing .csprojs in the .sln is done each time the MyApplication.Versioning.csproj 
	is built (or rebuilt). Here's the auto versioning logic within SalesApplication.Versioning.csproj:

		 <!-- START DYNAMIC ASSEMBLY VERSIONING WORK-->
		  <!--MSBuild Community Tasks path as installed via the nuget package 'Install-Package MSBuildTasks'-->
		  <PropertyGroup>
			<MSBuildCommunityTasksPath>$(MSBuildThisFileDirectory)..\.build</MSBuildCommunityTasksPath>
			<GitPath>$(MSBuildThisFileDirectory)..\git-binaries</GitPath>
			<My-PropertiesDir>Properties</My-PropertiesDir>
		  </PropertyGroup>
		  <PropertyGroup>
			<!--this should only be incremented (starting at zero) for MAJOR application releases this should never be reset only incremented!-->
			<ManualMajorVersion>0</ManualMajorVersion>
			<!--3 digits should only be manually incremented (starting at zero) for feature releases-->
			<!--!this should be reset to '0' at the start of each caldenar Year-->
			<ManualMinorVersion>000</ManualMinorVersion>
		  </PropertyGroup>
		  <!--Import MSBuild Community Tasks library installed from Nuget -->
		  <!--This library contains defined MSBUILD tasks useful for dynamically generating Assembly information via MSBUILD https://github.com/loresoft/msbuildtasks-->
		  <Import Project="$(MSBuildCommunityTasksPath)\MSBuild.Community.Tasks.Targets" />
		  <Target Name="BeforeBuild">
			<!-- Load the git commit hash; we will put this within the [AssemblyInformationalVersion] in order to track the asssembly version to the git commit-->
			<Message Importance="high" Text="Git $(GitPath) ..."/>
			<!-- Retrieve the Git commit hash, long version and store as the 'GitRevision' property-->
			<GitVersion LocalPath="$(GitPath)" Short="false">
			  <Output TaskParameter="CommitHash" PropertyName="GitRevision" />
			</GitVersion>
			<Time Format="yy.MM.dd.HHmm" Kind="Utc">
			  <Output TaskParameter="FormattedTime" PropertyName="My-VersionNumber" />
			</Time>
			<Message Text="Auto versioning from UTC time: $(My-VersionNumber) ...">
			</Message>
			<Time Format="yy" Kind="Utc">
			  <Output TaskParameter="FormattedTime" PropertyName="Year" />
			</Time>
			<Time Format="MM" Kind="Utc">
			  <Output TaskParameter="FormattedTime" PropertyName="Month" />
			</Time>
			<Time Format="dd" Kind="Utc">
			  <Output TaskParameter="FormattedTime" PropertyName="Day" />
			</Time>
			<Time Format="HH" Kind="Utc">
			  <Output TaskParameter="FormattedTime" PropertyName="Hour" />
			</Time>
			<Time Format="mm" Kind="Utc">
			  <Output TaskParameter="FormattedTime" PropertyName="Minute" />
			</Time>
			<ItemGroup>
			  <My-AssemblyInfo Include="$(My-PropertiesDir)\AutoVersion.cs" />
			  <Compile Include="@(My-AssemblyInfo)" />
			</ItemGroup>
			<MakeDir Directories="$(My-PropertiesDir)" />
			<PropertyGroup>
			  <GeneratedAssemblyVersion>$(ManualMajorVersion).$(Year)$(ManualMinorVersion).$(Month)$(Day).$(Hour)$(Minute)</GeneratedAssemblyVersion>
			</PropertyGroup>
			<AssemblyInfo OutputFile="@(My-AssemblyInfo)" CodeLanguage="CS" AssemblyFileVersion="$(GeneratedAssemblyVersion)" AssemblyInformationalVersion="$(GitRevision)" AssemblyVersion="$(GeneratedAssemblyVersion)" Condition="$(GeneratedAssemblyVersion) != '' " />
		  </Target>
		  <!-- END DYNAMIC ASSEMBLY VERSIONING WORK-->

	  ///////////////////////////////////////
	 /Satellite Consumers of AutoVersioning/
	///////////////////////////////////////

	Every .csproj in the .sln should subscribes to this dynamic, build-time assembly versioning through a custom, manual addition to their .csproj file:

  <!-- START DYNAMIC ASSEMBLY VERSIONING WORK-->
    <PropertyGroup>
    <!-- The file path output by SalesApplication.Versioning.csproj -->
    <Input-PropertiesDir>..\SalesApplication.Versioning\Properties</Input-PropertiesDir>
    <!-- The file output by SalesApplication.Versioning.csproj -->
    <Input-VersionFileName>AutoVersion.cs</Input-VersionFileName>
    </PropertyGroup>
    <Target Name="BeforeBuild">
    <ItemGroup>
	    <My-AssemblyInfo Include="$(Input-PropertiesDir)\$(Input-VersionFileName)" />
	    <Compile Include="@(My-AssemblyInfo)">
	    </Compile>
    </ItemGroup>
    </Target>
  <!-- END DYNAMIC ASSEMBLY VERSIONING WORK-->

	The above should be placed directly beneath:

		<Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />

	The .csproj can be manually modified by first unloading the project from the solution from within the Solution Explorer (right-click, "unload project") and then 
	right-click and selecting "edit project".

	These assemblies must also have their AssemblyInfo.cs' pruned of these Assembly attributes:

		[AssemblyFileVersion]
		[AssemblyInformationalVersion]
		[AssemblyVersion]

	  ////////////////////////
	 /Testing AutoVersioning/
	////////////////////////

	There is an automated integration test to verify that this logic works as expected, and that consuming assemblies reflect it through the build process within 
	the SalesApplication.Test assembly. Additionally the SalesApplication.Test.csproj has additional logic to copy the output versioning file created by 
	SalesApplication.Versioning.csproj to its own output directory/Testing/ in order to more easily consume it for testing at runtime. You can see how this is done 
	by right-clicking/unloading the test.csproj within VisualStudio and then editing the .csproj.

	  //////////////////////////////
	 /Strong-Naming the Assemblies/
	//////////////////////////////

	All .csprojs that consume SalesApplication.Versioning.csproj's assembly versioning functionality should also be strongly named to follow our convention of avoiding .dll hell.
	To do this:

		1.) Within Visual Studio, right-click the .csproj file and select 'Properties'.
		2.) Navigate to the 'Signing' section.
		3.) Click the checkbox 'Sign the assembly'.
		4.) Open the drop down beneath 'Choose a strong name key file:' and select 'New'.
		5.) Verify the checkbox specifying the key file is password protected is UNCHECKED.
		6.) Name the key as: "SalesApplication.NEW_NAME_HERE.Strong.pfx" (see other .csprojs /Properties/*.pfx files for naming convention examples).
		7.) From Solution Explorer, move the created key file into to the "Properties" folder within the .csproj.

	Note: After building the .csproj, every other assembly that this assembly references must be Strongly-Named!
		  (Currently the NancyFx assemblies that the SalesApplication consumes are unsigned, so projects dependent on Nancy cannot be Strongly Named -- yet)
