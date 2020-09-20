# Hash Calculator

Hash Calculator lets you calculate the cryptographic hash values of any file using CRC32, MD5, SHA-1, SHA-256, SHA-384, and SHA-512 algorithms.\
\
It relies on CertUtil by default (except for CRC32 hash calculation) but a switch can be used to call internal (built-in) cryptographic hash methods instead (however, please note that this can be slower than relying on CertUtil for large files).\
\
Download Hash Calculator and see it for yourself today!\
\
[![Hash-Calculator-v1.0.0.0.jpg](/Hash%Calculator%20v1.0.0.0.jpg)](https://github.com/clementcampagna/Hash-Calculator/releases/download/v1.0.0.0/Hash.Calculator.v1.0.0.0.zip)

### Usage:

You must pass at least one file path to this program in order to calculate its hash values.\
\
For example:
```
To calculate all hash values of a file (i.e. CRC32, MD5, SHA-1, SHA-256, SHA-384 and SHA-512):
"Hash Calculator.exe" "C:\Folder Path That Contains Spaces\a file.exe"

To calculate only the CRC32 et MD5 hash values of a file:
"Hash Calculator.exe" -crc32 -md5 "C:\a file.doc"

To calculate only the SHA-256 hash value of two files using the built in algorithm:
"Hash Calculator.exe" -sha256 -use-built-in "C:\text file.txt" C:\music_file.mp3
```

List of switch parameters that can be used:
```
-crc32        (to return only the CRC32 hash value of a file)
-md5          (to return only the MD5 hash value of a file)
-sha1         (to return only the SHA-1 hash value of a file)
-sha256       (to return only the SHA-256 hash value of a file)
-sha384       (to return only the SHA-384 hash value of a file)
-sha512       (to return only the SHA-512 hash value of a file)
-use-built-in (to calculate hash values using built-in algorithms instead of calling the CertUtil utility)
-h            (to display this help message)
--help        (to display this help message)

All switches can be combined together (e.g. -crc32 -md5 -use-built-in) at the exception of -h and --help
```

### Development:

Want to contribute? Great, Hash Calculator is open-source!\
\
Please feel free to clone this repository, fork it, make changes to the code, submit pull requests, raise issues, and/or email me using the address below for any suggestions, questions or remarks you may have.

### Contact Information:

Author's email address: clementcampagna+github@gmail.com\
Author's website: https://clementcampagna.com