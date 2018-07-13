FROM alpine

RUN apk add --update \
        bash \
        curl \
	wget\
    && rm -rf /var/cache/apk/*
COPY makerequest.sh .
RUN chmod +x makerequest.sh

CMD ["/bin/bash"]
