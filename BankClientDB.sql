
---------------insert data
INSERT INTO public."Role"(
	"Id", name)
	VALUES ('b2c182c6-3163-44f7-8321-75a56a7ed755', 'User');
INSERT INTO public."Role"(
	"Id", name)
	VALUES ('9343c4b2-16b9-4763-b72e-e0a5014acf0d', 'Admin');

INSERT INTO public."User"(
	"Id", name, phone, "Password", "Salt", "RoleId")
	VALUES ('8095e383-e3a5-4ff0-bd72-9d2c52af03c5', 'Kudlenok SA', '+79052338678', '���-���.�#t��z�G�S��AZ��Ž�˔�R�S�!�����Q�B�j��#��', '411279e0-76a1-45a5-ba47-8036f4364be8', '9343c4b2-16b9-4763-b72e-e0a5014acf0d'); 

INSERT INTO public."Bill"(
	"Id", name, value, "UserId")
	VALUES ('061146d0-4fd5-4212-b68e-68324e7d7822', '2', '2', '8095e383-e3a5-4ff0-bd72-9d2c52af03c5');

INSERT INTO public."Bill"(
	"Id", name, value, "UserId")
	VALUES ('4fb02e0b-84be-4d10-860e-2df187579b78', '1', '1', '8095e383-e3a5-4ff0-bd72-9d2c52af03c5');

INSERT INTO public."Bill"(
	"Id", name, value, "UserId")
	VALUES ('82739c9a-5950-44c0-afab-2ab03cf743b0', '3', '3', '8095e383-e3a5-4ff0-bd72-9d2c52af03c5');