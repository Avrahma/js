/**
 * 更新日期：2024-04-05 15:30:15
 * 用法：Sub-Store 脚本操作添加
 * rename.js 参数说明请参考原始脚本注释
 */

// 获取输入参数
const inArg = $arguments; // console.log(inArg)
const nx = inArg.nx || false,
  bl = inArg.bl || false,
  nf = inArg.nf || false,
  key = false, // 强制关闭 key 过滤，避免没有国旗节点被过滤
  blgd = inArg.blgd || false,
  blpx = inArg.blpx || false,
  blnx = inArg.blnx || false,
  numone = inArg.one || false,
  debug = inArg.debug || false,
  clear = inArg.clear || false,
  addflag = inArg.flag || false,
  nm = true; // 保留未匹配节点

const FGF = inArg.fgf == undefined ? " " : decodeURI(inArg.fgf),
  XHFGF = inArg.sn == undefined ? " " : decodeURI(inArg.sn),
  FNAME = inArg.name == undefined ? "" : decodeURI(inArg.name),
  BLKEY = inArg.blkey == undefined ? "" : decodeURI(inArg.blkey),
  blockquic = inArg.blockquic == undefined ? "" : decodeURI(inArg.blockquic),
  nameMap = {
    cn: "cn",
    zh: "cn",
    us: "us",
    en: "us",
    quan: "quan",
    gq: "gq",
    flag: "gq",
  },
  inname = nameMap[inArg.in] || "",
  outputName = nameMap[inArg.out] || "";

// 节点国家数组（简化显示，完整数组可直接用你原来的）
const FG = ['🇭🇰','🇲🇴','🇹🇼','🇯🇵','🇰🇷','🇸🇬','🇺🇸','🇬🇧','🇫🇷','🇩🇪','🇦🇺','🇦🇪','🇦🇫','🇦🇱','🇩🇿','🇦🇴','🇦🇷','🇦🇲','🇦🇹','🇦🇿','🇧🇭','🇧🇩','🇧🇾','🇧🇪','🇧🇿','🇧🇯','🇧🇹','🇧🇴','🇧🇦','🇧🇼','🇧🇷','🇻🇬','🇧🇳','🇧🇬','🇧🇫','🇧🇮','🇰🇭','🇨🇲','🇨🇦','🇨🇻','🇰🇾','🇨🇫','🇹🇩','🇨🇱','🇨🇴','🇰🇲','🇨🇬','🇨🇩','🇨🇷','🇭🇷','🇨🇾','🇨🇿','🇩🇰','🇩🇯','🇩🇴','🇪🇨','🇪🇬','🇸🇻','🇬🇶','🇪🇷','🇪🇪','🇪🇹','🇫🇯','🇫🇮','🇬🇦','🇬🇲','🇬🇪','🇬🇭','🇬🇷','🇬🇱','🇬🇹','🇬🇳','🇬🇾','🇭🇹','🇭🇳','🇭🇺','🇮🇸','🇮🇳','🇮🇩','🇮🇷','🇮🇶','🇮🇪','🇮🇲','🇮🇱','🇮🇹','🇨🇮','🇯🇲','🇯🇴','🇰🇿','🇰🇪','🇰🇼','🇰🇬','🇱🇦','🇱🇻','🇱🇧','🇱🇸','🇱🇷','🇱🇾','🇱🇹','🇱🇺','🇲🇰','🇲🇬','🇲🇼','🇲🇾','🇲🇻','🇲🇱','🇲🇹','🇲🇷','🇲🇺','🇲🇽','🇲🇩','🇲🇨','🇲🇳','🇲🇪','🇲🇦','🇲🇿','🇲🇲','🇳🇦','🇳🇵','🇳🇱','🇳🇿','🇳🇮','🇳🇪','🇳🇬','🇰🇵','🇳🇴','🇴🇲','🇵🇰','🇵🇦','🇵🇾','🇵🇪','🇵🇭','🇵🇹','🇵🇷','🇶🇦','🇷🇴','🇷🇺','🇷🇼','🇸🇲','🇸🇦','🇸🇳','🇷🇸','🇸🇱','🇸🇰','🇸🇮','🇸🇴','🇿🇦','🇪🇸','🇱🇰','🇸🇩','🇸🇷','🇸🇿','🇸🇪','🇨🇭','🇸🇾','🇹🇯','🇹🇿','🇹🇭','🇹🇬','🇹🇴','🇹🇹','🇹🇳','🇹🇷','🇹🇲','🇻🇮','🇺🇬','🇺🇦','🇺🇾','🇺🇿','🇻🇪','🇻🇳','🇾🇪','🇿🇲','🇿🇼','🇦🇩','🇷🇪','🇵🇱','🇬🇺','🇻🇦','🇱🇮','🇨🇼','🇸🇨','🇦🇶','🇬🇮','🇨🇺','🇫🇴','🇦🇽','🇧🇲','🇹🇱']
// prettier-ignore
const EN = ['HK','MO','TW','JP','KR','SG','US','GB','FR','DE','AU','AE','AF','AL','DZ','AO','AR','AM','AT','AZ','BH','BD','BY','BE','BZ','BJ','BT','BO','BA','BW','BR','VG','BN','BG','BF','BI','KH','CM','CA','CV','KY','CF','TD','CL','CO','KM','CG','CD','CR','HR','CY','CZ','DK','DJ','DO','EC','EG','SV','GQ','ER','EE','ET','FJ','FI','GA','GM','GE','GH','GR','GL','GT','GN','GY','HT','HN','HU','IS','IN','ID','IR','IQ','IE','IM','IL','IT','CI','JM','JO','KZ','KE','KW','KG','LA','LV','LB','LS','LR','LY','LT','LU','MK','MG','MW','MY','MV','ML','MT','MR','MU','MX','MD','MC','MN','ME','MA','MZ','MM','NA','NP','NL','NZ','NI','NE','NG','KP','NO','OM','PK','PA','PY','PE','PH','PT','PR','QA','RO','RU','RW','SM','SA','SN','RS','SL','SK','SI','SO','ZA','ES','LK','SD','SR','SZ','SE','CH','SY','TJ','TZ','TH','TG','TO','TT','TN','TR','TM','VI','UG','UA','UY','UZ','VE','VN','YE','ZM','ZW','AD','RE','PL','GU','VA','LI','CW','SC','AQ','GI','CU','FO','AX','BM','TL'];
// prettier-ignore
const ZH = ['【高级】香港','【高级】澳门','【高级】台湾','【高级】日本','【高级】韩国','【高级】新加坡','【高级】美国','【高级】英国','【高级】法国','【高级】德国','【高级】澳大利亚','【高级】阿联酋','【高级】阿富汗','阿尔巴尼亚','阿尔及利亚','安哥拉','【高级】阿根廷','亚美尼亚','【高级】奥地利','阿塞拜疆','【高级】巴林','孟加拉国','白俄罗斯','比利时','伯利兹','贝宁','不丹','玻利维亚','波斯尼亚和黑塞哥维那','博茨瓦纳','【高级】巴西','英属维京群岛','文莱','保加利亚','布基纳法索','布隆迪','【高级】柬埔寨','喀麦隆','【高级】加拿大','佛得角','开曼群岛','中非共和国','乍得','智利','【高级】哥伦比亚','科摩罗','刚果(布)','刚果(金)','哥斯达黎加','克罗地亚','塞浦路斯','捷克','丹麦','吉布提','多米尼加共和国','厄瓜多尔','埃及','萨尔瓦多','赤道几内亚','厄立特里亚','爱沙尼亚','埃塞俄比亚','斐济','芬兰','加蓬','冈比亚','格鲁吉亚','加纳','希腊','格陵兰','危地马拉','几内亚','圭亚那','海地','洪都拉斯','匈牙利','冰岛','【高级】印度','【高级】印尼','伊朗','伊拉克','【高级】爱尔兰','马恩岛','以色列','【高级】意大利','科特迪瓦','牙买加','约旦','【高级】哈萨克斯坦','肯尼亚','科威特','吉尔吉斯斯坦','【高级】老挝','拉脱维亚','黎巴嫩','莱索托','利比里亚','利比亚','【高级】立陶宛','【高级】卢森堡','马其顿','马达加斯加','马拉维','【高级】马来','【高级】马尔代夫','马里','马耳他','毛利塔尼亚','毛里求斯','【高级】墨西哥','摩尔多瓦','摩纳哥','蒙古','黑山共和国','摩洛哥','莫桑比克','缅甸','纳米比亚','尼泊尔','【高级】荷兰','【高级】新西兰','尼加拉瓜','尼日尔','尼日利亚','【高级】朝鲜','挪威','阿曼','巴基斯坦','巴拿马','巴拉圭','秘鲁','【高级】菲律宾','【高级】葡萄牙','波多黎各','卡塔尔','罗马尼亚','【高级】俄罗斯','卢旺达','圣马力诺','沙特阿拉伯','塞内加尔','塞尔维亚','塞拉利昂','斯洛伐克','斯洛文尼亚','索马里','【高级】南非','【高级】西班牙','斯里兰卡','苏丹','苏里南','斯威士兰','【高级】瑞典','【高级】瑞士','叙利亚','塔吉克斯坦','【高级】坦桑尼亚','泰国','多哥','汤加','特立尼达和多巴哥','突尼斯','土耳其','土库曼斯坦','美属维尔京群岛','乌干达','乌克兰','乌拉圭','乌兹别克斯坦','委内瑞拉','【高级】越南','也门','赞比亚','津巴布韦','安道尔','留尼汪','【高级】波兰','关岛','梵蒂冈','列支敦士登','库拉索','塞舌尔','南极','直布罗陀','古巴','法罗群岛','奥兰群岛','百慕达','东帝汶'];
// prettier-ignore
const QC = ['Hong Kong','Macao','Taiwan','Japan','Korea','Singapore','United States','United Kingdom','France','Germany','Australia','Dubai','Afghanistan','Albania','Algeria','Angola','Argentina','Armenia','Austria','Azerbaijan','Bahrain','Bangladesh','Belarus','Belgium','Belize','Benin','Bhutan','Bolivia','Bosnia and Herzegovina','Botswana','Brazil','British Virgin Islands','Brunei','Bulgaria','Burkina-faso','Burundi','Cambodia','Cameroon','Canada','CapeVerde','CaymanIslands','Central African Republic','Chad','Chile','Colombia','Comoros','Congo-Brazzaville','Congo-Kinshasa','CostaRica','Croatia','Cyprus','Czech Republic','Denmark','Djibouti','Dominican Republic','Ecuador','Egypt','EISalvador','Equatorial Guinea','Eritrea','Estonia','Ethiopia','Fiji','Finland','Gabon','Gambia','Georgia','Ghana','Greece','Greenland','Guatemala','Guinea','Guyana','Haiti','Honduras','Hungary','Iceland','India','Indonesia','Iran','Iraq','Ireland','Isle of Man','Israel','Italy','Ivory Coast','Jamaica','Jordan','Kazakstan','Kenya','Kuwait','Kyrgyzstan','Laos','Latvia','Lebanon','Lesotho','Liberia','Libya','Lithuania','Luxembourg','Macedonia','Madagascar','Malawi','Malaysia','Maldives','Mali','Malta','Mauritania','Mauritius','Mexico','Moldova','Monaco','Mongolia','Montenegro','Morocco','Mozambique','Myanmar(Burma)','Namibia','Nepal','Netherlands','New Zealand','Nicaragua','Niger','Nigeria','NorthKorea','Norway','Oman','Pakistan','Panama','Paraguay','Peru','Philippines','Portugal','PuertoRico','Qatar','Romania','Russia','Rwanda','SanMarino','SaudiArabia','Senegal','Serbia','SierraLeone','Slovakia','Slovenia','Somalia','SouthAfrica','Spain','SriLanka','Sudan','Suriname','Swaziland','Sweden','Switzerland','Syria','Tajikstan','Tanzania','Thailand','Togo','Tonga','TrinidadandTobago','Tunisia','Turkey','Turkmenistan','U.S.Virgin Islands','Uganda','Ukraine','Uruguay','Uzbekistan','Venezuela','Vietnam','Yemen','Zambia','Zimbabwe','Andorra','Reunion','Poland','Guam','Vatican','Liechtensteins','Curacao','Seychelles','Antarctica','Gibraltar','Cuba','Faroe Islands','Ahvenanmaa','Bermuda','Timor-Leste'];

// 保留/倍率/正则数组
const specialRegex = [ /(\d\.)?\d+×/, /IPLC|IEPL|Kern|Edge|Pro|Std|Exp|Biz|Fam|Game|Buy|Zx|LB|Game/ ];
const regexArray=[/ˣ²/, /ˣ³/, /ˣ⁴/, /ˣ⁵/, /ˣ⁶/, /ˣ⁷/, /ˣ⁸/, /ˣ⁹/, /ˣ¹⁰/, /ˣ²⁰/, /ˣ³⁰/, /ˣ⁴⁰/, /ˣ⁵⁰/, /IPLC/i, /IEPL/i, /核心/, /边缘/, /高级/, /标准/, /实验/, /商宽/, /家宽/, /游戏|game/i, /购物/, /专线/, /LB/, /cloudflare/i, /\budp\b/i, /\bgpt\b/i,/udpn\b/];
const valueArray= [ "2×","3×","4×","5×","6×","7×","8×","9×","10×","20×","30×","40×","50×","IPLC","IEPL","Kern","Edge","Pro","Std","Exp","Biz","Fam","Game","Buy","Zx","LB","CF","UDP","GPT","UDPN"];
const nameblnx = /(高倍|(?!1)2+(x|倍)|ˣ²|ˣ³|ˣ⁴|ˣ⁵|ˣ¹⁰)/i;
const namenx = /(高倍|(?!1)(0\.|\d)+(x|倍)|ˣ²|ˣ³|ˣ⁴|ˣ⁵|ˣ¹⁰)/i;

// 匹配国家关键字的对象
const rurekey = {
  德国: /(深|沪|呼|京|广|杭)德(?!.*(I|线))|法兰克福|滬德/g,
  日本: /(深|沪|呼|京|广|杭|中|辽)日(?!.*(I|线))|东京|大坂/g,
  新加坡: /狮城|(深|沪|呼|京|广|杭)新/g,
  美国: /(深|沪|呼|京|广|杭)美|波特兰|芝加哥|哥伦布|纽约|硅谷|俄勒冈|西雅图|芝加哥/g
  // 其他可继续按原脚本补充
};

let GetK = false, AMK = [];
function ObjKA(i) { GetK = true; AMK = Object.entries(i); }

// 核心处理函数
function operator(pro) {
  const Allmap = {};
  const outList = getList(outputName);
  let inputList;
  if (inname !== "") {
    inputList = [getList(inname)];
  } else {
    inputList = [ZH, FG, QC, EN];
  }

  inputList.forEach((arr) => {
    arr.forEach((value, valueIndex) => {
      Allmap[value] = outList[valueIndex];
    });
  });

  // 过滤逻辑（关键过滤已关闭 key=false，nm=true 保留未匹配）
  if (clear || nx || blnx) {
    pro = pro.filter((res) => {
      const resname = res.name;
      const shouldKeep =
        !(clear && /(套餐|到期|有效|剩余|版本|已用|过期|失联|测试)/i.test(resname)) &&
        !(nx && namenx.test(resname)) &&
        !(blnx && !nameblnx.test(resname));
      return shouldKeep;
    });
  }

  const BLKEYS = BLKEY ? BLKEY.split("+") : "";

  pro.forEach((e) => {
    let bktf = false, ens = e.name;

    // 匹配地区关键字
    Object.keys(rurekey).forEach((ikey) => {
      if (rurekey[ikey].test(e.name)) {
        e.name = e.name.replace(rurekey[ikey], ikey);
      }
    });

    // 自定义保留关键字
    if (BLKEY) {
      let BLKEY_REPLACE = "", re = false;
      BLKEYS.forEach((i) => {
        if (i.includes(">") && e.name.includes(i.split(">")[0])) {
          if (i.split(">")[1]) { BLKEY_REPLACE = i.split(">")[1]; re = true; }
        }
      });
      const retainKey = re ? BLKEY_REPLACE : BLKEYS.filter((items) => e.name.includes(items));
      e.name = e.name + (retainKey.length ? " " + retainKey.join(" ") : "");
    }

    // block-quic 配置
    if (blockquic == "on") e["block-quic"] = "on";
    else if (blockquic == "off") e["block-quic"] = "off";
    else delete e["block-quic"];

    // 处理未匹配的节点，保证 nm=true 时保留
    if (!e.name && nm) e.name = FNAME + (FNAME ? FGF : "") + ens;
  });

  jxh(pro);
  numone && oneP(pro);
  blpx && (pro = fampx(pro));
  return pro;
}

// 获取输出名称数组
function getList(arg) { switch (arg) { case 'us': return EN; case 'gq': return FG; case 'quan': return QC; default: return ZH; }}

// 序号处理函数
function jxh(e) {
  const n = e.reduce((e, n) => {
    const t = e.find((e) => e.name === n.name);
    if (t) { t.count++; t.items.push({ ...n, name: `${n.name}${XHFGF}${t.count.toString().padStart(2, "0")}` }); }
    else { e.push({ name: n.name, count: 1, items: [{ ...n, name: `${n.name}${XHFGF}01` }] }); }
    return e;
  }, []);
  const t = n.reduce((acc, e) => acc.concat(e.items), []);
  e.splice(0, e.length, ...t);
  return e;
}

// 单节点序号处理
function oneP(e) { const t = e.reduce((e, t) => { const n = t.name.replace(/[^A-Za-z0-9\u00C0-\u017F\u4E00-\u9FFF]+\d+$/, ""); if (!e[n]) e[n]=[]; e[n].push(t); return e; }, {}); for (const e in t) { if (t[e].length===1 && t[e][0].name.endsWith("01")) t[e][0].name=t[e][0].name.replace(/[^.]01/,""); } return e; }

// 排序函数
function fampx(pro) {
  const wis=[], wnout=[];
  for(const proxy of pro) specialRegex.some((regex)=>regex.test(proxy.name))?wis.push(proxy):wnout.push(proxy);
  const sps = wis.map((proxy)=>specialRegex.findIndex((regex)=>regex.test(proxy.name)));
  wis.sort((a,b)=>sps[wis.indexOf(a)]-sps[wis.indexOf(b)]||a.name.localeCompare(b.name));
  wnout.sort((a,b)=>pro.indexOf(a)-pro.indexOf(b));
  return wnout.concat(wis);
}
