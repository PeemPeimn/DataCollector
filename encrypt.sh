PASSPHRASE=$1

zip -r ./Database/.env.zip ./Database/.env

gpg --batch --yes --symmetric --cipher-algo AES256 --passphrase $PASSPHRASE --quiet ./Tests/Reddit.Tests/appsettings.test.json
gpg --batch --yes --symmetric --cipher-algo AES256 --passphrase $PASSPHRASE --quiet ./Database/.env.zip 