# Keycloak for SoccerManager, backed by SQL Server.
#
# The realm JSON is baked in so the same image imports the same realm locally
# and in Azure. Build options are fixed at image build time, which is what
# lets the container start with --optimized. The login theme is baked in the
# same way the realm is.

FROM quay.io/keycloak/keycloak:26.2

COPY realm/ /opt/keycloak/data/import/
COPY themes/ /opt/keycloak/themes/

RUN /opt/keycloak/bin/kc.sh build --db=mssql --health-enabled=true

ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]
CMD ["start", "--import-realm", "--optimized"]
