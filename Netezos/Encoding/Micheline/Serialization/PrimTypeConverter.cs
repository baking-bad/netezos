using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Encoding.Serialization
{
    public class PrimTypeConverter : JsonConverter<PrimType>
    {
        public override PrimType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.GetString())
            {
                case "parameter": return PrimType.parameter;
                case "storage": return PrimType.storage;
                case "code": return PrimType.code;
                case "False": return PrimType.False;
                case "Elt": return PrimType.Elt;
                case "Left": return PrimType.Left;
                case "None": return PrimType.None;
                case "Pair": return PrimType.Pair;
                case "Right": return PrimType.Right;
                case "Some": return PrimType.Some;
                case "True": return PrimType.True;
                case "Unit": return PrimType.Unit;
                case "PACK": return PrimType.PACK;
                case "UNPACK": return PrimType.UNPACK;
                case "BLAKE2B": return PrimType.BLAKE2B;
                case "SHA256": return PrimType.SHA256;
                case "SHA512": return PrimType.SHA512;
                case "ABS": return PrimType.ABS;
                case "ADD": return PrimType.ADD;
                case "AMOUNT": return PrimType.AMOUNT;
                case "AND": return PrimType.AND;
                case "BALANCE": return PrimType.BALANCE;
                case "CAR": return PrimType.CAR;
                case "CDR": return PrimType.CDR;
                case "CHECK_SIGNATURE": return PrimType.CHECK_SIGNATURE;
                case "COMPARE": return PrimType.COMPARE;
                case "CONCAT": return PrimType.CONCAT;
                case "CONS": return PrimType.CONS;
                case "CREATE_ACCOUNT": return PrimType.CREATE_ACCOUNT;
                case "CREATE_CONTRACT": return PrimType.CREATE_CONTRACT;
                case "IMPLICIT_ACCOUNT": return PrimType.IMPLICIT_ACCOUNT;
                case "DIP": return PrimType.DIP;
                case "DROP": return PrimType.DROP;
                case "DUP": return PrimType.DUP;
                case "EDIV": return PrimType.EDIV;
                case "EMPTY_MAP": return PrimType.EMPTY_MAP;
                case "EMPTY_SET": return PrimType.EMPTY_SET;
                case "EQ": return PrimType.EQ;
                case "EXEC": return PrimType.EXEC;
                case "FAILWITH": return PrimType.FAILWITH;
                case "GE": return PrimType.GE;
                case "GET": return PrimType.GET;
                case "GT": return PrimType.GT;
                case "HASH_KEY": return PrimType.HASH_KEY;
                case "IF": return PrimType.IF;
                case "IF_CONS": return PrimType.IF_CONS;
                case "IF_LEFT": return PrimType.IF_LEFT;
                case "IF_NONE": return PrimType.IF_NONE;
                case "INT": return PrimType.INT;
                case "LAMBDA": return PrimType.LAMBDA;
                case "LE": return PrimType.LE;
                case "LEFT": return PrimType.LEFT;
                case "LOOP": return PrimType.LOOP;
                case "LSL": return PrimType.LSL;
                case "LSR": return PrimType.LSR;
                case "LT": return PrimType.LT;
                case "MAP": return PrimType.MAP;
                case "MEM": return PrimType.MEM;
                case "MUL": return PrimType.MUL;
                case "NEG": return PrimType.NEG;
                case "NEQ": return PrimType.NEQ;
                case "NIL": return PrimType.NIL;
                case "NONE": return PrimType.NONE;
                case "NOT": return PrimType.NOT;
                case "NOW": return PrimType.NOW;
                case "OR": return PrimType.OR;
                case "PAIR": return PrimType.PAIR;
                case "PUSH": return PrimType.PUSH;
                case "RIGHT": return PrimType.RIGHT;
                case "SIZE": return PrimType.SIZE;
                case "SOME": return PrimType.SOME;
                case "SOURCE": return PrimType.SOURCE;
                case "SENDER": return PrimType.SENDER;
                case "SELF": return PrimType.SELF;
                case "STEPS_TO_QUOTA": return PrimType.STEPS_TO_QUOTA;
                case "SUB": return PrimType.SUB;
                case "SWAP": return PrimType.SWAP;
                case "TRANSFER_TOKENS": return PrimType.TRANSFER_TOKENS;
                case "SET_DELEGATE": return PrimType.SET_DELEGATE;
                case "UNIT": return PrimType.UNIT;
                case "UPDATE": return PrimType.UPDATE;
                case "XOR": return PrimType.XOR;
                case "ITER": return PrimType.ITER;
                case "LOOP_LEFT": return PrimType.LOOP_LEFT;
                case "ADDRESS": return PrimType.ADDRESS;
                case "CONTRACT": return PrimType.CONTRACT;
                case "ISNAT": return PrimType.ISNAT;
                case "CAST": return PrimType.CAST;
                case "RENAME": return PrimType.RENAME;
                case "bool": return PrimType.@bool;
                case "contract": return PrimType.contract;
                case "int": return PrimType.@int;
                case "key": return PrimType.key;
                case "key_hash": return PrimType.key_hash;
                case "lambda": return PrimType.lambda;
                case "list": return PrimType.list;
                case "map": return PrimType.map;
                case "big_map": return PrimType.big_map;
                case "nat": return PrimType.nat;
                case "option": return PrimType.option;
                case "or": return PrimType.or;
                case "pair": return PrimType.pair;
                case "set": return PrimType.set;
                case "signature": return PrimType.signature;
                case "string": return PrimType.@string;
                case "bytes": return PrimType.bytes;
                case "mutez": return PrimType.mutez;
                case "timestamp": return PrimType.timestamp;
                case "unit": return PrimType.unit;
                case "operation": return PrimType.operation;
                case "address": return PrimType.address;
                case "SLICE": return PrimType.SLICE;
                case "DIG": return PrimType.DIG;
                case "DUG": return PrimType.DUG;
                case "EMPTY_BIG_MAP": return PrimType.EMPTY_BIG_MAP;
                case "APPLY": return PrimType.APPLY;
                case "chain_id": return PrimType.chain_id;
                case "CHAIN_ID": return PrimType.CHAIN_ID;
                case "LEVEL": return PrimType.LEVEL;
                case "SELF_ADDRESS": return PrimType.SELF_ADDRESS;
                case "never": return PrimType.never;
                case "NEVER": return PrimType.NEVER;
                case "UNPAIR": return PrimType.UNPAIR;
                case "VOTING_POWER": return PrimType.VOTING_POWER;
                case "TOTAL_VOTING_POWER": return PrimType.TOTAL_VOTING_POWER;
                case "KECCAK": return PrimType.KECCAK;
                case "SHA3": return PrimType.SHA3;
                case "PAIRING_CHECK": return PrimType.PAIRING_CHECK;
                case "bls12_381_g1": return PrimType.bls12_381_g1;
                case "bls12_381_g2": return PrimType.bls12_381_g2;
                case "bls12_381_fr": return PrimType.bls12_381_fr;
                case "sapling_state": return PrimType.sapling_state;
                case "sapling_transaction": return PrimType.sapling_transaction;
                case "SAPLING_EMPTY_STATE": return PrimType.SAPLING_EMPTY_STATE;
                case "SAPLING_VERIFY_UPDATE": return PrimType.SAPLING_VERIFY_UPDATE;
                case "ticket": return PrimType.ticket;
                case "TICKET": return PrimType.TICKET;
                case "READ_TICKET": return PrimType.READ_TICKET;
                case "SPLIT_TICKET": return PrimType.SPLIT_TICKET;
                case "JOIN_TICKETS": return PrimType.JOIN_TICKETS;
                case "GET_AND_UPDATE": return PrimType.GET_AND_UPDATE;
                default:
                    throw new FormatException("Unknown prim type");
            }
        }

        public override void Write(Utf8JsonWriter writer, PrimType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Enum.GetName(typeof(PrimType), value));
        }
    }
}
