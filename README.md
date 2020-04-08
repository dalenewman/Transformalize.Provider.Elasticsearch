### Overview

This is an `Elasticsearch` provider for [Transformalize](https://github.com/dalenewman/Transformalize) 
using [Elasticsearch.Net](https://github.com/elastic/elasticsearch-net) version 5.6.6.  It has been tested and works 
with Elasticsearch 4 through 7.  Be sure to set the version attribute accordingly.  

### Write Usage

```xml
<add name='TestProcess' mode='init'>
  <connections>
    <add name='input' provider='bogus' seed='1' />
    <add name='output' provider='elasticsearch' index='bogus' server='localhost' port='9200' version='7.6.2' />
  </connections>
  <entities>
    <add name='Contact' size='1000'>
      <fields>
        <add name='Identity' type='int' primary-key='true' />
        <add name='FirstName' />
        <add name='LastName' />
        <add name='Stars' type='byte' min='1' max='5' />
        <add name='Reviewers' type='int' min='0' max='500' />
      </fields>
    </add>
  </entities>
</add>
```

This writes 1000 rows of bogus data to an Elasticsearch 6 index.

### Read Usage

```xml
<add name='TestProcess' >
  <connections>
    <add name='input' provider='elasticsearch' index='bogus' port='9200' version='6' />
  </connections>
  <entities>
    <add name='contact' page='1' size='10'>
      <order>
        <add field='identity' />
      </order>
      <fields>
        <add name='identity' type='int' />
        <add name='firstname' />
        <add name='lastname' />
        <add name='stars' type='byte' />
        <add name='reviewers' type='int' />
      </fields>
    </add>
  </entities>
</add>
```

This reads 10 rows of bogus data from an Elasticsearch 6 index:

<pre>
<strong>identity,firstname,lastname,stars,reviewers</strong>
1,Justin,Konopelski,3,153
2,Eula,Schinner,2,41
3,Tanya,Shanahan,4,412
4,Emilio,Hand,4,469
5,Rachel,Abshire,3,341
6,Doyle,Beatty,4,458
7,Delbert,Durgan,2,174
8,Harold,Blanda,4,125
9,Willie,Heaney,5,342
10,Sophie,Hand,2,176</pre>

### Notes

- Field names go into Elasticsearch as lower case.
- The entity's name is used as the elasticsearch `_type` field when writing, and used as a filter when reading.   
- If using a multi-node cluster, you may specify servers in your connection with `url` or (`name` and `port`) attributes:

```xml
<connections>
   <add name="x" provider="elasticsearch" index="y" version="7.6.2">
      <servers>
         <add url="http://node1:9200" />
         <add url="http://node2:9200" />
      </servers>
   </add>
</connections>
```