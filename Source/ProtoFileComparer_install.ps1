param($installPath, $toolsPath, $package, $project)

$folder = $project.ProjectItems.Item("ProtoTools")

$file1 = $folder.ProjectItems.Item("protoc.exe")
$file2 = $folder.ProjectItems.Item("protoc-license.txt")

# set 'Copy To Output Directory' to 'Copy if newer'
$copyToOutput1 = $file1.Properties.Item("CopyToOutputDirectory")
$copyToOutput1.Value = 2

$copyToOutput2 = $file2.Properties.Item("CopyToOutputDirectory")
$copyToOutput2.Value = 2