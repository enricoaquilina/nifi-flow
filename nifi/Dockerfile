FROM apache/nifi:1.14.0
USER root
RUN  apt-get update \
  && apt-get install -y wget \
  && rm -rf /var/lib/apt/lists/*

RUN apt install wget

USER nifi

RUN wget https://repo1.maven.org/maven2/org/postgresql/postgresql/42.2.23/postgresql-42.2.23.jar
RUN cp ./postgresql-42.2.23.jar /opt/nifi/nifi-current/lib/postgresql-42.2.23.jar

