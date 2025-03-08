REM nuget pack Transformalize.Provider.Elasticsearch.nuspec -OutputDirectory "c:\temp\modules"
REM nuget pack Transformalize.Provider.Elasticsearch.Autofac.nuspec -OutputDirectory "c:\temp\modules"

nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.0.11.1-beta.nupkg" -source https://www.myget.org/F/transformalize/api/v3/index.json
nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.Autofac.0.11.1-beta.nupkg" -source https://www.myget.org/F/transformalize/api/v3/index.json
