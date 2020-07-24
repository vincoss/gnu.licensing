

## Issues (Fix OPENSSL)
set OPENSSL_CONF=C:\Program Files\OpenSSL-Win64\bin\openssl.cfg
openssl version

## Install
Local Machine

## Create Certificate (Pass@word1)

Create the .key and .cer files:
openssl req –newkey rsa:4096 –nodes –keyout Gnu.Licensing.key –x509 –days 3365 –out Gnu.Licensing.cer

From these files create the .pfx:
openssl pkcs12 –export –in Gnu.Licensing.cer –inkey Gnu.Licensing.key –out Gnu.Licensing.pfx -name "Gnu.Licensing"