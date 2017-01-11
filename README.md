[![NuGet Version](http://img.shields.io/nuget/v/GeekLearning.D64.svg?style=flat-square)](https://www.nuget.org/packages/GeekLearning.D64/)
[![Build Status](https://geeklearning.visualstudio.com/_apis/public/build/definitions/f841b266-7595-4d01-9ee1-4864cf65aa73/15/badge)](#)

# Dotnet D64

This library is a .net standard port of [d64](https://github.com/dominictarr/d64). 
d64 is a copy-pastable, url friendly, ascii embeddable, lexiographicly sortable binary encoding.

## D64Convert

D64Convert is a class providing feature very similar to native .net `Convert.ToBase64String` and `Convert.FromBase64String()`.
It exposes two static methods.

### Encode 

```csharp
static string Encode(byte[] data)
```

Encode takes a byte array and produces a D64 encoded string.

### Decode 

```csharp
static byte[] Decode(string str)
```

Decode takes a D64 encoded string and returns the decoded byte array.

## TimeBasedId

TimeBaseId is a sortable likely to be unique id generator. It is similar to guids except that they would be time sortable.

### Constructor 

```csharp
TimebasedId(bool reverseOrder)
```

You can specify if you want the natural sorting order to be reversed. This might be extremely usefull when using table storage and you want the data to be ordered from most recent to oldest.

### New ID 

```csharp
string NewId()
```

Generates a new Id based on `DateTimeOffset.UtcNow`.

```csharp
string NewId(DateTimeOffset datetime)
```

Generates a new Id using the specified `DateTimeOffset`.

### DateBoundary
```csharp
string DateBoundat(DateTimeOffset datetime)
```
Generates a boundary string which can be used in comparisons to find more recent or older ids.
