#!/bin/bash

PASSWORD=""
FILENAME=""


for arg in "$@"
do
	case $arg in
		-f|--file)
		FILENAME="$2"
		shift
		shift
		;;
		-p|--password)
		PASSWORD="$2"
		shift
		shift
		;;
	esac
	
done

if [ -n "$FILENAME" ] && [ -n "$PASSWORD" ]; then
	sudo dotnet dev-certs https -ep $HOME/.aspnet/https/$FILENAME -p $PASSWORD
else
	echo "Usage: -f|--filename [FILENAME].pfx -p|--password [PASSWORD]"
fi	

