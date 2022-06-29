using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    public class TxRollupRejectionContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "tx_rollup_rejection";

        [JsonPropertyName("rollup")]
        public string Rollup { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("message")]
        public TxRollupRejectionMessage Message { get; set; }

        [JsonPropertyName("message_position")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long MessagePosition { get; set; }

        [JsonPropertyName("message_path")]
        public List<string> MessagePath { get; set; } // var

        [JsonPropertyName("message_result_hash")]
        public string MessageResultHash { get; set; } // 32

        [JsonPropertyName("message_result_path")]
        public List<string> MessageResultPath { get; set; } // var

        [JsonPropertyName("previous_message_result")]
        public TxRollupMessageResult PreviousMessageResult { get; set; }

        [JsonPropertyName("previous_message_result_path")]
        public List<string> PreviousMessageResultPath { get; set; } // var

        [JsonPropertyName("proof")]
        public JsonElement Proof { get; set; }
    }

    public class TxRollupRejectionMessage
    {
        [JsonPropertyName("batch")]
        public string Batch { get; set; } // var

        [JsonPropertyName("deposit")]
        public TxRollupRejectionDepositMessage Deposit { get; set; }
    }

    public class TxRollupRejectionDepositMessage
    {
        [JsonPropertyName("sender")]
        public string Sender { get; set; } // 21

        [JsonPropertyName("destination")]
        public string Destination { get; set; } // 20

        [JsonPropertyName("ticket_hash")]
        public string TicketHash { get; set; } // 32

        [JsonPropertyName("amount")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Amount { get; set; }
    }

    public class TxRollupMessageResult
    {
        [JsonPropertyName("context_hash")]
        public string ContextHash { get; set; } // 32

        [JsonPropertyName("withdraw_list_hash")]
        public string WithdrawListHash { get; set; } // 32
    }

    public abstract class TxRollupProof
    {
        public abstract byte Tag { get; }

        public short Field0 { get; set; }

        public byte[] Field1 { get; set; } // 32

        public byte[] Field2 { get; set; } // 32

        public X_15 Field3 { get; set; }
    }

    public class X_139_0 : TxRollupProof
    {
        public override byte Tag => 0;
    }

    public class X_139_1 : TxRollupProof
    {
        public override byte Tag => 1;
    }

    public class X_139_2 : TxRollupProof
    {
        public override byte Tag => 2;
    }

    public class X_139_3 : TxRollupProof
    {
        public override byte Tag => 3;
    }

    public class X_15
    {
        public X_138[] Field0 { get; set; } // var
    }

    public abstract class X_138
    {
        public abstract byte Tag { get; }
    }

    public class X_138_0 : X_138
    {
        public override byte Tag => 0;
        public byte Field0 { get; set; }
    }

    public class X_138_1 : X_138
    {
        public override byte Tag => 1;
        public short Field0 { get; set; }
    }

    public class X_138_2 : X_138
    {
        public override byte Tag => 2;
        public int Field0 { get; set; }
    }

    public class X_138_3 : X_138
    {
        public override byte Tag => 3;
        public long Field0 { get; set; }
    }

    public class X_138_4 : X_138
    {
        public override byte Tag => 4;
        public byte Field0 { get; set; }
        public X_133 Field1 { get; set; }   
    }

    public class X_138_5 : X_138
    {
        public override byte Tag => 5;
        public short Field0 { get; set; }
        public X_133 Field1 { get; set; }
    }

    public class X_138_6 : X_138
    {
        public override byte Tag => 6;
        public int Field0 { get; set; }
        public X_133 Field1 { get; set; }
    }

    public class X_138_7 : X_138
    {
        public override byte Tag => 7;
        public long Field0 { get; set; }
        public X_133 Field1 { get; set; }
    }

    public class X_138_8 : X_138
    {
        public override byte Tag => 8;
        public byte Field0 { get; set; }
        public X_133 Field1 { get; set; }
    }

    public class X_138_9 : X_138
    {
        public override byte Tag => 9;
        public short Field0 { get; set; }
        public X_133 Field1 { get; set; }
    }

    public class X_138_10 : X_138
    {
        public override byte Tag => 10;
        public int Field0 { get; set; }
        public X_133 Field1 { get; set; }
    }

    public class X_138_11 : X_138
    {
        public override byte Tag => 11;
        public long Field0 { get; set; }
        public X_133 Field1 { get; set; }
    }

    public class X_138_12 : X_138
    {
        public override byte Tag => 12;
        public byte Field0 { get; set; }
        public X_125 Field1 { get; set; }
    }

    public class X_138_13 : X_138
    {
        public override byte Tag => 13;
        public short Field0 { get; set; }
        public X_125 Field1 { get; set; }
    }

    public class X_138_14 : X_138
    {
        public override byte Tag => 14;
        public int Field0 { get; set; }
        public X_125 Field1 { get; set; }
    }

    public class X_138_15 : X_138
    {
        public override byte Tag => 15;
        public long Field0 { get; set; }
        public X_125 Field1 { get; set; }
    }

    public class X_138_128 : X_138
    {
        public override byte Tag => 128;
    }

    public class X_138_129 : X_138
    {
        public override byte Tag => 129;
        public X_20[] Field0 { get; set; } // 1
    }

    public class X_138_130 : X_138
    {
        public override byte Tag => 130;
        public X_20[] Field0 { get; set; } // 2
    }

    public class X_138_131 : X_138
    {
        public override byte Tag => 131;
        public X_20[] Field0 { get; set; } // var
    }

    public class X_138_192 : X_138
    {
        public override byte Tag => 192;
        public byte[] Field0 { get; set; } // 1-byte var
    }

    public class X_138_193 : X_138
    {
        public override byte Tag => 193;
        public byte[] Field0 { get; set; } // 2-bytes var
    }

    public class X_138_195 : X_138
    {
        public override byte Tag => 195;
        public byte[] Field0 { get; set; } // 4-bytes var
    }

    public class X_138_224 : X_138
    {
        public override byte Tag => 224;
        public byte Field0 { get; set; }
        public X_120 Field1 { get; set; }
        public byte[] Field2 { get; set; } // 32
    }

    public class X_138_225 : X_138
    {
        public override byte Tag => 225;
        public short Field0 { get; set; }
        public X_120 Field1 { get; set; }
        public byte[] Field2 { get; set; } // 32
    }

    public class X_138_226 : X_138
    {
        public override byte Tag => 226;
        public int Field0 { get; set; }
        public X_120 Field1 { get; set; }
        public byte[] Field2 { get; set; } // 32
    }

    public class X_138_227 : X_138
    {
        public override byte Tag => 227;
        public long Field0 { get; set; }
        public X_120 Field1 { get; set; }
        public byte[] Field2 { get; set; } // 32
    }

    public class X_133
    {
        public byte[] Field0 { get; set; } // 32
    }

    public class X_125
    {
        public byte[] Field0 { get; set; } // 32
        public byte[] Field1 { get; set; } // 32
    }

    public class X_20
    {
        public X_120 Field0 { get; set; }

        public X_121 Field1 { get; set; }
    }

    public class X_120
    {
        public byte[] Field0 { get; set; } // var
    }

    public abstract class X_121
    {
        public abstract byte Tag { get; }
        public byte[] ContextHash { get; set; } // 32
    }

    public class X_121_0 : X_121
    {
        public override byte Tag => 0;
    }

    public class X_121_1 : X_121
    {
        public override byte Tag => 1;
    }
}
