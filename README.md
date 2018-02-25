### Overview

This adds a `Elasticsearch` provider to Transformalize using [ElasticsearchNet](https://github.com/ElasticsearchNet/ElasticsearchNet).  It is a plug-in compatible with Transformalize 0.3.3-beta.

Build the Autofac project and put it's output into Transformalize's *plugins* folder.

### Write Usage

```xml
<add name='TestProcess' mode='init'>
  <connections>
    <add name='input' provider='bogus' seed='1' />
    <add name='output' provider='Elasticsearch' core='bogus' folder='c:\java\Elasticsearch-6.2.1\cores' path='Elasticsearch' port='8983' />
  </connections>
  <entities>
    <add name='Contact' size='1000'>
      <fields>
        <add name='FirstName' />
        <add name='LastName' />
        <add name='Stars' type='byte' min='1' max='5' />
        <add name='Reviewers' type='int' min='0' max='500' />
      </fields>
    </add>
  </entities>
</add>
```

This writes 1000 rows of bogus data to a Elasticsearch 6 core at *c:\java\Elasticsearch-6.2.1\cores\bogus*.

### Read Usage

```xml
<add name='TestProcess' >
  <connections>
    <add name='input' provider='Elasticsearch' core='bogus' folder='c:\java\Elasticsearch-6.2.1\cores' path='Elasticsearch' port='8983' />
  </connections>
  <entities>
    <add name='Contact' page='1' size='10'>
      <fields>
        <add name='firstname' />
        <add name='lastname' />
        <add name='stars' type='byte' />
        <add name='reviewers' type='int' />
      </fields>
    </add>
  </entities>
</add>
```

This reads 10 rows of bogus data from a Elasticsearch 6 core at *c:\java\Elasticsearch-6.2.1\cores\bogus*:

<pre>
<strong>firstname,lastname,stars,reviewers</strong>
Justin,Konopelski,3,153
Eula,Schinner,2,41
Tanya,Shanahan,4,412
Emilio,Hand,4,469
Rachel,Abshire,3,341
Doyle,Beatty,4,458
Delbert,Durgan,2,174
Harold,Blanda,4,125
Willie,Heaney,5,342
Sophie,Hand,2,176</pre>

### Notes

- Tested with Elasticsearch 6.
- Field names go into Elasticsearch as lower case.
- Uses older ElasticsearchNet at *https://ci.appveyor.com/nuget/Elasticsearchnet-022x5w7kmuba*.
