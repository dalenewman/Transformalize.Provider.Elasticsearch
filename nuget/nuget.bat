nuget pack Transformalize.Provider.Elasticsearch.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Provider.Elasticsearch.Autofac.nuspec -OutputDirectory "c:\temp\modules"

REM nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.0.6.4-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.Autofac.0.4.4-beta.nupkg" -source https://api.nuget.org/v3/index.json






