## General Guidelines

1. All code **MUST** conform to SOLID design principles.
2. All code **MUST** contain unit tests that will fail if critical sections of the code are removed.

## Deprecating an Operating System

- Remove the affected operating system from the ci.yml and publish.yml workflow build matrices
- Remove the host identifier from the DotRas.csproj file
- Remove the host identifier possible values from the netcoreandfx.props file
- Remove any preprocessor directives associated with the host identifier. 
  For example: A host identifier of win7 may have pre-processor directives of WIN7 in use within the codebase.