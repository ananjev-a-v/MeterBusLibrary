# MeterBusLibrary
Simple M-Bus connect and parse C# .NET library

Implementation of protocol described on http://www.m-bus.com/

#Usage:

Main (and most complex) kind of data from device is VariableData. This data type consisyts of single FixedDataHeader and many VariableData.Item. According to M-Bus protocol documentation this type consists of single data value (of variable length and type) and set of units which describes and specify value units. Direct usage of these data is not easy, so library normalizes most of data with NormalizedValue property.
