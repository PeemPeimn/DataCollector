PASSPHRASE=$1

gpg --quiet --batch --yes --decrypt --passphrase=$PASSPHRASE \
  --output ./Database/.env.zip ./Database/.env.zip.gpg
gpg --quiet --batch --yes --decrypt --passphrase=$PASSPHRASE \
  --output ./Tests/Reddit.Tests/appsettings.test.json ./Tests/Reddit.Tests/appsettings.test.json.gpg

unzip -o ./Database/.env.zip -d ./Database