REM --batch to prevent interactive command
REM --yes to assume "yes" for questions

gpg --quiet --batch --yes --decrypt --passphrase="$SNK_PASSPHRASE" --output Winnster.snk Winnster.snk.gpg