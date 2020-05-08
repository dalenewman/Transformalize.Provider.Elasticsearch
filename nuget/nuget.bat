nuget pack Transformalize.Provider.Elasticsearch.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Provider.Elasticsearch.Autofac.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Provider.Elasticsearch.Autofac.v3.nuspec -OutputDirectory "c:\temp\modules"


REM nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.0.7.6-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.Autofac.0.7.6-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.Autofac.v3.0.7.6-beta.nupkg" -source https://api.nuget.org/v3/index.json






