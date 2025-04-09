
CREATE TABLE users(
    id uuid NOT NULL,
    username varchar(100) NOT NULL UNIQUE,
    email VARCHAR(100) NOT null UNIQUE,
    password_hash text NOT NULL,
    CONSTRAINT pk_users PRIMARY KEY(id)
)

create unique index IX_USERS_Email on users(email);
create unique index IX_USERS_Username on usrrs(username);