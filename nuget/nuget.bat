nuget pack Transformalize.Provider.Elasticsearch.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Provider.Elasticsearch.Autofac.nuspec -OutputDirectory "c:\temp\modules"

nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.0.3.4-beta.nupkg" -source https://api.nuget.org/v3/index.json
nuget push "c:\temp\modules\Transformalize.Provider.Elasticsearch.Autofac.0.3.4-beta.nupkg" -source https://api.nuget.org/v3/index.json






