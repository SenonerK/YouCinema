-- Create ID Generator 
Create or replace function random_string(length integer) returns text as
$$
declare
  chars text[] := '{0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z}';
  result text := '';
  i integer := 0;
begin
  if length < 0 then
    raise exception '! < 0';
  end if;
  for i in 1..length loop
    result := result || chars[1+random()*(array_length(chars, 1)-1)];
  end loop;
  return result;
end;
$$ language plpgsql;
 
-- Create Tables
 
CREATE TABLE yc_customer 
(
    ID VARCHAR(5) DEFAULT random_string(5) PRIMARY KEY NOT NULL,
    email VARCHAR(50) NOT NULL,
    firstname VARCHAR(30) NOT NULL,
    lastname VARCHAR(40) NOT NULL,
    credit DOUBLE PRECISION NOT NULL,
    isDisabled BOOLEAN NOT NULL
);
CREATE TABLE yc_cast
(
    ID VARCHAR(6) DEFAULT random_string(6) PRIMARY KEY NOT NULL,
    firstname VARCHAR(30) NOT NULL,
    lastname VARCHAR(40) NOT NULL,
    date_of_birth DATE NOT NULL,
    IMDB_rating DOUBLE PRECISION NOT NULL
);
CREATE TABLE yc_movie
(
    ID VARCHAR(7) DEFAULT random_string(7) PRIMARY KEY NOT NULL,
    thumbnail VARCHAR(10) NOT NULL,
    description VARCHAR,
    m_name VARCHAR(50) NOT NULL,
    publishing_year DATE NOT NULL,
    price_per_day_borrow DOUBLE PRECISION NOT NULL,
    duration TIME NOT NULL
);
CREATE TABLE yc_rooms
(
    roomID VARCHAR(3) DEFAULT random_string(3) PRIMARY KEY NOT NULL,
    max_col INT NOT NULL,
    max_row INT NOT NULL,
    room_name VARCHAR(20) NOT NULL
);
CREATE TABLE yc_demonstration
(
    demonstrationID VARCHAR(4) DEFAULT random_string(4) PRIMARY KEY NOT NULL,
    ticket_prices DOUBLE PRECISION NOT NULL,
    demonstration_date TIMESTAMP NOT NULL,
    fk_movieID VARCHAR(7) NOT NULL,
    fk_roomID VARCHAR(3) NOT NULL,
    CONSTRAINT yc_demonstration_yc_movie_id_fk FOREIGN KEY (fk_movieID) REFERENCES yc_movie (id),
    CONSTRAINT yc_demonstration_yc_rooms_roomid_fk FOREIGN KEY (fk_roomID) REFERENCES yc_rooms (roomid)
);
CREATE TABLE yc_reserved
(
    ticketID VARCHAR(15) DEFAULT random_string(15) PRIMARY KEY NOT NULL,
    fk_customerID VARCHAR(5) NOT NULL,
    fk_demonstrationID VARCHAR(4) NOT NULL,
    pos INTEGER[] NOT NULL,
    CONSTRAINT yc_reserved_yc_customer_id_fk FOREIGN KEY (fk_customerID) REFERENCES yc_customer (id),
    CONSTRAINT yc_reserved_yc_demonstration_demonstrationid_fk FOREIGN KEY (fk_demonstrationID) REFERENCES yc_demonstration (demonstrationid)
);
CREATE TABLE yc_borrow
(
    BID VARCHAR(8) DEFAULT random_string(8) PRIMARY KEY,
    start_time TIMESTAMP DEFAULT now() NOT NULL,
    end_time TIMESTAMP NOT NULL,
    fk_customerID VARCHAR(5) NOT NULL,
    fk_movieID VARCHAR(7) NOT NULL,
    CONSTRAINT yc_borrow_yc_customer_id_fk FOREIGN KEY (fk_customerID) REFERENCES yc_customer (id),
    CONSTRAINT yc_borrow_yc_movie_id_fk FOREIGN KEY (fk_movieID) REFERENCES yc_movie (id)
);
CREATE TABLE yc_borrow_log
(
    ID SERIAL NOT NULL PRIMARY KEY,
    logdate TIMESTAMP NOT NULL,
    fk_movieID VARCHAR(7) NOT NULL,
    fk_customerID VARCHAR(5) NOT NULL,
    CONSTRAINT yc_borrow_log_yc_customer_id_fk FOREIGN KEY (fk_customerID) REFERENCES yc_customer (id),
    CONSTRAINT yc_borrow_log_yc_movie_id_fk FOREIGN KEY (fk_movieID) REFERENCES yc_movie (id)
);
CREATE TABLE yc_participations
(
    id VARCHAR(10) DEFAULT random_string(10) PRIMARY KEY NOT NULL,
    fk_actor VARCHAR(6) NOT NULL,
    fk_movie VARCHAR(7) NOT NULL,
    movie_role VARCHAR,
    CONSTRAINT yc_participations_yc_cast_id_fk FOREIGN KEY (fk_actor) REFERENCES yc_cast (id),
    CONSTRAINT yc_participations_yc_movie_id_fk FOREIGN KEY (fk_movie) REFERENCES yc_movie (id)
);
 
-- Create Auto Log Function
 
Create or replace function log_borrow()
 returns trigger as
$$
begin
  INSERT INTO yc_borrow_log(ID, logdate, fk_movieid, fk_customerid)
  VALUES (DEFAULT, now(), NEW.fk_movieid, NEW.fk_customerid);
 
return NEW;
end;
$$
LANGUAGE plpgsql;
 
-- Add Trigger to Function
 
CREATE TRIGGER logging
  AFTER INSERT
  ON yc_borrow
  FOR EACH ROW
  EXECUTE PROCEDURE log_borrow();