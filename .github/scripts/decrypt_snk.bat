gpg --version
gpg --quiet --batch --yes --decrypt --passphrase="$SNK_PASSPHRASE" --symmetric --cipher-algo AES256 --output Winnster.snk Winnster.snk.gpg