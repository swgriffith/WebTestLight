#!/bin/bash

RESP=$(curl -w %{time_total} -o /dev/null -s ${TARGET})
curl -d "" -X POST "${LOGURL}&region=${LOC}&resptime=${RESP}"
