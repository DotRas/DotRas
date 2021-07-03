gpg --version
gpg --quiet --batch --yes --passphrase %SNK_PASSPHRASE% --output Winnster.snk -d Winnster.snk.gpg