using BOTWM.Server.DataTypes;

namespace BOTWM.Server.DTO
{
    public class BumiiDTO
    {
        public ffsdDTO ffsd;
        public bodyDTO body;
        public personalDTO personal;
        public commonDTO common;
        public shapeDTO shape;
        public hairDTO hair;
        public eyeDTO eye;
        public eye_ctrlDTO eye_ctrl;
        public eyebrowDTO eyebrow;
        public noseDTO nose;
        public mouthDTO mouth;
        public beardDTO beard;
        public glassDTO glass;
        public korogDTO korog;
        public goronDTO goron;
        public gerudoDTO gerudo;
        public ritoDTO rito;
        public zoraDTO zora;
    }

    public class ffsdDTO
    {
        public bool no_use_ffsd;
        public short type;
    }

    public class bodyDTO
    {
        public short race;
        public short type;
        public short number;
        public short weight;
        public short height;
    }

    public class personalDTO
    {
        public short sex_age;
        public short fav_color;
        public short sub_color_1;
        public short sub_color_2;
        public short head_fav_color;
        public short shoulder_fav_color;
        public short shoulder_sub_color_1;
    }

    public class commonDTO
    {
        public short backpack;
        public short hat;
        public bool no_hat_always;
        public short body_correct;
        public bool is_mid_age;
        public float rot_cravicle;
        public float rot_arm;
        public float rot_leg;
        public float rot_crotch;
    }

    public class shapeDTO
    {
        public short jaw;
        public short wrinkle;
        public short make;
        public float trans_v;
        public float scale;
        public short skin_color;
    }

    public class hairDTO
    {
        public short type;
        public short color;
        public bool flip;
    }

    public class eyeDTO
    {
        public short type;
        public short color;
        public float trans_v;
        public float trans_u;
        public float rotate;
        public float scale;
        public float aspect;
        public float eyeball_trans_u;
        public float eyeball_trans_v;
        public float eyeball_scale;
        public short highlight_bright;
    }

    public class eye_ctrlDTO
    {
        public Vec3f base_offset;
        public float translim_out;
        public float translim_in;
        public float translim_d;
        public float translim_u;
        public float neck_offset_ud;
    }

    public class eyebrowDTO
    {
        public short type;
        public short color;
        public float trans_v;
        public float trans_u;
        public float rotate;
        public float scale;
        public float aspect;
    }

    public class noseDTO
    {
        public short type;
        public float trans_v;
        public float scale;
    }

    public class mouthDTO
    {
        public short type;
        public short color;
        public float trans_v;
        public float scale;
        public float aspect;
    }

    public class beardDTO
    {
        public short mustache;
        public float scale;
        public short type;
        public short color;
    }

    public class glassDTO
    {
        public short type;
        public short color;
    }

    public class korogDTO
    {
        public short mask;
        public short skin_color;
        public short left_plant;
        public short right_plant;
    }

    public class goronDTO
    {
        public short skin_color;
    }

    public class gerudoDTO
    {
        public short hair;
        public short hair_color;
        public short glass;
        public short glass_color;
        public short skin_color;
        public short lip_color;
    }

    public class ritoDTO
    {
        public short body_color;
        public short hair_color;
    }

    public class zoraDTO
    {
        public short body_color;
    }
}
