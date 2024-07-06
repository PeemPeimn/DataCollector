PASSPHRASE=$1

gpg --quiet --batch --yes --decrypt --passphrase=$PASSPHRASE \
  --output ./Database/.env.zip ./Database/.env.zip.gpg
gpg --quiet --batch --yes --decrypt --passphrase=$PASSPHRASE \
  --output ./Tests/Reddit.Tests/appsettings.test.json ./Tests/Reddit.Tests/appsettings.test.json.gpg
grep 'Connection' ./Tests/Reddit.Tests/appsettings.test.json | awk -F ";" '{print $1}'
unzip -o ./Database/.env.zip