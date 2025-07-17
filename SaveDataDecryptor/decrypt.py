import sqlite3

KEY_FOR_SAVE_KEY = bytes([
    0x63, 0x13, 0x15, 0xF3, 0x25, 0x22, 0x86, 0x93, 0x12, 0x32, 0x47, 0x3F,
    0x2A, 0x6A, 0xB3, 0x01, 0x51, 0xE1, 0x17, 0x71, 0xF1, 0x82, 0xAA, 0xB1,
    0x24, 0x6B, 0x46, 0xA3, 0x45, 0xF2, 0x3F, 0x57, 0xD3, 0x90, 0x50, 0x37,
    0x42, 0xC7, 0x46, 0xB0, 0x70, 0x07, 0xC5, 0x22
])

KEY_FOR_SAVE_VALUE = bytes([
    0x17, 0x5B, 0x2A, 0x48, 0x18, 0xD3, 0xE2, 0x9F, 0xCC, 0xEA, 0x2B, 0x35,
    0x51, 0x27, 0x85, 0x33, 0xDC, 0x11, 0xBA, 0x6D, 0x61, 0x09, 0xF2, 0x42,
    0x87, 0x56, 0xF9, 0x0D, 0xBB, 0x14, 0x1A, 0xB9, 0x36, 0x81, 0xFE, 0x49,
    0x26, 0xDA, 0x64, 0x97, 0x32, 0x58, 0xBF, 0xCD
])


def xor_decode(data: bytes, key: bytes) -> bytes:
    if not key:
        raise ValueError("XOR key is not provided.")
    
    decoded = bytearray()
    for i in range(len(data)):
        decoded.append(data[i] ^ key[i % len(key)])
    return bytes(decoded)


def read_and_decode_save_data(db_path: str, table_name: str):
    """
    Connects to the SQLite DB, reads encoded key-value pairs as BLOBs,
    decrypts them using the correct XOR keys, and prints the result.
    """
    try:
        con = sqlite3.connect(db_path)
        cur = con.cursor()

        query = f"SELECT `save_key`, `save_value` FROM `{table_name}`"
        
        print(f"Executing query on table '{table_name}'...")
        cur.execute(query)
        
        print("\n--- Decoded SaveData.db ---")
        
        for row in cur.fetchall():
            encoded_key = row[0]
            encoded_value = row[1]
            
            try:
                decrypted_key_bytes = xor_decode(encoded_key, KEY_FOR_SAVE_KEY)
                decrypted_value_bytes = xor_decode(encoded_value, KEY_FOR_SAVE_VALUE)

                key_str = decrypted_key_bytes.decode('utf-8')
                value_str = decrypted_value_bytes.decode('utf-8')

                print(f"{key_str}: {value_str}")

            except Exception as e:
                print(f"Could not process row. Key: {encoded_key.hex()}, Value: {encoded_value.hex()}. Error: {e}")

    except sqlite3.Error as e:
        print(f"Database error: {e}")
    finally:
        if 'con' in locals() and con:
            con.close()
            print("\nDatabase connection closed.")

read_and_decode_save_data("SaveData.db", "AppSetting")