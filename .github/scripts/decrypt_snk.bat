gpg --version
gpg --quiet --batch --yes --passphrase %SNK_PASSPHRASE% --output Winnster.snk -d ./.github/secrets/Winnster.snk.gpg