# --batch to prevent interactive command
# --yes to assume "yes" for questions
gpg --quiet --batch --yes --decrypt --passphrase="$SNK_PASSPHRASE" --output Winnster.snk Winnster.snk.gpg