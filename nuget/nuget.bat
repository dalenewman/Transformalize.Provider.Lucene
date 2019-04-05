nuget pack Transformalize.Provider.Lucene.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Provider.Lucene.Autofac.nuspec -OutputDirectory "c:\temp\modules"

REM nuget push "c:\temp\modules\Transformalize.Provider.Lucene.0.5.0-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Provider.Lucene.Autofac.0.5.0-beta.nupkg" -source https://api.nuget.org/v3/index.json






